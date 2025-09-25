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
using common.data.Enums;

namespace ia.api.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors("Procent")]
    [Authorize]
    [ApiController]
    public class PromoCodesController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IPromoCodeRepository _promoCodeRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public PromoCodesController(IMapper mapper, IPromoCodeRepository promoCodeRepository,
           ILogger logger, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _promoCodeRepository = promoCodeRepository;
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
        }

        [Authorize(Roles = CustomPolicies.LIManager)]
        [HttpPost, Route("add")]
        public async Task<IActionResult> add([FromBody] PromoCodeBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();

                var promoCode = _mapper.Map<PromoCode>(model);

                promoCode.CreatorUserId = userId;

                var addedPromoCode = await _promoCodeRepository.Add(promoCode, userId);
                if (addedPromoCode == null)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotSaved("PromoCode")}", false, null));

                var pvm = _mapper.Map<PromoCodeViewModel>(addedPromoCode);

                var username = await _userManager.FindByIdAsync(userId);

                pvm.CreatorName = $"{username?.FirstName} {username?.LastName}";

                return Ok(new ResponseModel($"{CustomMessages.Saved("PromoCode")}", false, pvm));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = CustomPolicies.Administrator)]
        [HttpPut, Route("activate")]
        public async Task<IActionResult> Activate([FromBody] CrudModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();

                var addedEmailTemplate = await _promoCodeRepository.Activate(model.Id, userId);
                if (!addedEmailTemplate)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotUpdated("PromoCode")}", false, null));

                return Ok(new ResponseModel($"{CustomMessages.Updated("PromoCode")}", false, 1));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete, Route("delete")]
        public async Task<IActionResult> Delete([FromBody] CrudModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();
                var deletedDocument = await _promoCodeRepository.GetSingle(model.Id, userId);
                if (deletedDocument == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("PromoCode")}", false, null));

                deletedDocument = await _promoCodeRepository.Delete(deletedDocument.Id, userId);
                if (deletedDocument == null)
                    return BadRequest(new ResponseModel($"{CustomMessages.StringMessage("Unable to delete document")}", false, null));

                var pvm = _mapper.Map<PromoCodeViewModel>(deletedDocument);

                var username = await _userManager.FindByIdAsync(userId);

                pvm.CreatorName = $"{username?.FirstName} {username?.LastName}";

                return Ok(new ResponseModel($"{CustomMessages.Deleted("PromoCode")}", false, deletedDocument));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = CustomPolicies.Administrator)]
        [HttpGet, Route("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = GetUserId();

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("User")}", false, null));

                var promoCodes = new List<PromoCode>();
                if (user.UserRoleEnum == UserRoleEnum.Administrator)
                    promoCodes = _promoCodeRepository.GetAll().ToList();
                else
                    promoCodes = _promoCodeRepository.Get(userId).ToList();

                var ivms = _mapper.Map<List<PromoCodeViewModel>>(promoCodes);

                foreach (var ivm in ivms)
                {
                    var promoCode = promoCodes.Where(p => p.Id == ivm.Id).FirstOrDefault();

                    var username = await _userManager.FindByIdAsync(promoCode.CreatorUserId);

                    ivm.CreatorName = $"{username?.FirstName} {username?.LastName}";
                }

                return Ok(new ResponseModel($"{CustomMessages.Fetched(ivms.Count().ToString(), "PromoCode(s)")}", false, ivms));
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

                var promoCode = await _promoCodeRepository.GetSingle(model.Id, userId);
                if (promoCode == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("PromoCode")}", false, null));

                var pvm = _mapper.Map<PromoCodeViewModel>(promoCode);

                var username = await _userManager.FindByIdAsync(userId);

                pvm.CreatorName = $"{username?.FirstName} {username?.LastName}";

                return Ok(new ResponseModel($"{CustomMessages.Fetched("1", "PromoCode")}", false, pvm));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost, Route("markUser")]
        public async Task<IActionResult> MarkUser([FromBody] CrudModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();

                var promoCode = await _promoCodeRepository.GetSingle(model.Id, userId);
                if (promoCode == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("PromoCode")}", false, null));

                promoCode.UsedPromoCode(new UserPromoCode()
                {
                    UserId = userId,
                    PromoCodeId = promoCode.Id
                });

                var updatedDocument = await _promoCodeRepository.Update(promoCode.Id, promoCode, userId);
                if (updatedDocument == null)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotUpdated("PromoCode")}", false, null));

                var pvm = _mapper.Map<PromoCodeViewModel>(promoCode);

                var username = await _userManager.FindByIdAsync(userId);

                pvm.CreatorName = $"{username?.FirstName} {username?.LastName}";

                return Ok(new ResponseModel($"{CustomMessages.Fetched("1", "PromoCode")}", false, pvm));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut, Route("update")]
        public async Task<IActionResult> Update([FromBody] PromoCodeUpdateBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();

                var singlePromoCode = await _promoCodeRepository.GetSingle(model.Id, userId);
                if (singlePromoCode == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("PromoCode")}", false, null));

                var promoCode = _mapper.Map<PromoCode>(model);
                var updatedDocument = await _promoCodeRepository.Update(promoCode.Id, promoCode, userId);
                if (updatedDocument == null)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotUpdated("PromoCode")}", false, null));

                var pvm = _mapper.Map<PromoCodeViewModel>(updatedDocument);

                var username = await _userManager.FindByIdAsync(userId);

                pvm.CreatorName = $"{username?.FirstName} {username?.LastName}";

                return Ok(new ResponseModel($"{CustomMessages.Updated("PromoCode")}", false, pvm));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}