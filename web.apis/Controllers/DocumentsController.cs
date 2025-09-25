using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using web.apis;
using data.models;
using web.apis.Controllers;
using web.apis.Models;
using ILogger = Serilog.ILogger;
using common.data;

namespace ia.api.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors("Procent")]
    [Authorize]
    [ApiController]
    public class DocumentsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IDocumentRepository _documentService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IS3UploadFileRepository _uploadFileRepository;

        public DocumentsController(IMapper mapper, IDocumentRepository documentService,
           ILogger logger, IConfiguration configuration, UserManager<ApplicationUser> userManager, IS3UploadFileRepository uploadFileService)
        {
            _mapper = mapper;
            _documentService = documentService;
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
            _uploadFileRepository = uploadFileService;
        }

        [HttpDelete, Route("delete")]
        public async Task<IActionResult> Delete([FromBody] CrudModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();
                var deletedDocument = await _documentService.GetSingle(model.Id);
                if (deletedDocument == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("Document")}", false, null));

                deletedDocument = await _documentService.Delete(deletedDocument.Id, userId);
                if (deletedDocument == null)
                    return BadRequest(new ResponseModel($"{CustomMessages.StringMessage("Unable to delete document")}", false, null));

                var dvm = _mapper.Map<DocumentViewModel>(deletedDocument);

                return Ok(new ResponseModel($"{CustomMessages.Deleted("Document")}", false, deletedDocument));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet, Route("get")]
        public IActionResult Get()
        {
            try
            {
                var userId = GetUserId();

                var documents = _documentService.Get(userId);

                Int32.TryParse(_configuration.GetValue<string>("S3:AccessDuration"), out int accessDuration);
                documents = _documentService.CheckExpiry(documents.ToList(), accessDuration);

                var ivms = _mapper.Map<List<DocumentViewModel>>(documents);

                return Ok(new ResponseModel($"{CustomMessages.Fetched(ivms.Count().ToString(), "Document")}", false, ivms));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost, Route("getSingle")]
        public async Task<IActionResult> GetSingle([FromBody] CrudModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();

                var document = await _documentService.GetSingle(model.Id);
                if (document == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("Document")}", false, null));

                var ivm = _mapper.Map<DocumentViewModel>(document);
                // check if the document has not expired and if it has, then regenerate the document and update
                Int32.TryParse(_configuration.GetValue<string>("S3:AccessDuration"), out int accessDuration);
                var expiryDate = DateTime.UtcNow.AddHours(accessDuration);
                if (expiryDate > DateTime.UtcNow)
                {
                    // regenerate url
                    var documentUrl = ""; // _uploadFileService.GeneratePreSignedUrl(document.ActualFileName);

                    // update document
                    if (document != null)
                    {
                        document.FileName = documentUrl;
                        document.ExpiryDate = expiryDate;

                        await _documentService.Update(document.Id, document, userId);
                    }
                }

                var dvm = _mapper.Map<DocumentViewModel>(document);

                return Ok(new ResponseModel($"{CustomMessages.Fetched("1", "Document")}", false, dvm));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut, Route("update")]
        public async Task<IActionResult> Update([FromBody] DocumentUpdateBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();

                var addedDocument = await _documentService.GetSingle(model.Id);
                if (addedDocument == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("Document")}", false, null));

                var Document = _mapper.Map<Document>(model);
                var updatedDocument = await _documentService.Update(Document.Id, Document, userId);
                if (updatedDocument == null)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotUpdated("Document")}", false, null));

                var dvm = _mapper.Map<DocumentViewModel>(updatedDocument);

                return Ok(new ResponseModel($"{CustomMessages.Updated("Document")}", false, dvm));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost, Route("uploadProfilePicture")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> UploadFile([FromForm] ValueFile file)
        {
            try
            {
                ApplicationUser selectedUser;
                var userId = GetUserId();

                selectedUser = await _userManager.FindByIdAsync(userId);

                if (string.IsNullOrWhiteSpace(userId))
                    return BadRequest(new ResponseModel($"{CustomMessages.StringMessage("User Id not supplied")}", false, null));

                if (file != null && file.File.Length > 0)
                {
                    // if maximum file size exceeds that from the api, reject
                    var maxSize = _configuration.GetValue<int>("MaximumFileUpload");
                    if (file.File.Length > maxSize)
                        return BadRequest(new ResponseModel($"{CustomMessages.StringMessage("File size too big")}", false, null));

                    FileInfo fi = new FileInfo(file.File.FileName);

                    // GetByStatus File Name  
                    var extension = fi.Extension;

                    var allowedFileExtentions = _configuration.GetValue<string>("FileUpload:AllowedExtensions").Split(',');
                    if (!allowedFileExtentions.Contains(fi.Extension))
                        return BadRequest(new ResponseModel($"{CustomMessages.StringMessage("File extension not allowed")}", false, null));

                    var newFileName = $"{Security.RandomString(12)}{extension}";

                    //var s3UploadModel = new { UploadStatus = true, Url = "" };// await _uploadFileService.UploadFile(newFileName, file.File);
                    var s3UploadModel = await _uploadFileRepository.UploadFile(newFileName, file.File);
                    if (s3UploadModel.UploadStatus)
                    {
                        Int32.TryParse(_configuration.GetValue<string>("S3:AccessDuration"), out int accessDuration);

                        var document = new Document(DocumentType.ProfilePicture, newFileName, file.File.FileName, s3UploadModel.Url, userId, accessDuration, 0, "", "");
                        var doc = await _documentService.Add(document, userId);

                        var dvm = _mapper.Map<DocumentViewModel>(document);

                        var user = await _userManager.FindByIdAsync(userId);

                        user.AddDocument(doc);
                        var res = await _userManager.UpdateAsync(user);

                        dvm.UserViewModel = _mapper.Map<UserViewModel>(user);
                        dvm.Path = s3UploadModel.Url;

                        return Ok(new ResponseModel($"{CustomMessages.Uploaded("Document")}", false, dvm));
                    }

                    return BadRequest(new ResponseModel($"{CustomMessages.NotUploaded("Document")}", false, null));
                }

                return BadRequest(new ResponseModel($"{CustomMessages.StringMessage("File cannot be empty")}", false, null));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}