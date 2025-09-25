using AutoMapper;
using common.data;
using data.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using web.apis;
using web.apis.Controllers;
using web.apis.Models;

[Route("api/v1/[controller]")]
[EnableCors("Procent")]
[Authorize(Roles = CustomPolicies.Everyone)]
public class EmailTemplatesController : BaseController
{
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public EmailTemplatesController(IConfiguration configuration, IEmailTemplateRepository emailTemplateRepository,
        IMapper mapper)
    {
        _configuration = configuration;
        _emailTemplateRepository = emailTemplateRepository;
        _mapper = mapper;
    }

    [Authorize(Roles = CustomPolicies.LIManager)]
    [HttpPost, Route("add")]
    public async Task<IActionResult> add([FromBody] EmailTemplateBindingModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

            var userId = GetUserId();

            var emailTemplate = _mapper.Map<EmailTemplate>(model);
            var addedEmailTemplate = await _emailTemplateRepository.Add(emailTemplate, userId);
            if (addedEmailTemplate == null)
                return BadRequest(new ResponseModel($"{CustomMessages.NotSaved("Email Template")}", false, null));

            var fvm = _mapper.Map<EmailTemplateViewModel>(addedEmailTemplate);

            return Ok(new ResponseModel($"{CustomMessages.Saved("Email Template")}", false, 1));
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

            var addedEmailTemplate = await _emailTemplateRepository.Activate(model.Id, userId);
            if (!addedEmailTemplate)
                return BadRequest(new ResponseModel($"{CustomMessages.NotUpdated("Email Template")}", false, null));

            return Ok(new ResponseModel($"{CustomMessages.Updated("Email Template")}", false, 1));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize(Roles = CustomPolicies.LIManager)]
    [HttpDelete, Route("delete")]
    public async Task<ActionResult> Delete([FromBody] CrudModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

            var userId = GetUserId();

            var singleEmailTemplate = await _emailTemplateRepository.GetById(model.Id);
            if (singleEmailTemplate == null)
                return NotFound(new ResponseModel($"{CustomMessages.NotFound("Email Template")}", false, null));

            var deletedEmailTemplate = await _emailTemplateRepository.Delete(singleEmailTemplate.Id, userId);

            var fvm = _mapper.Map<EmailTemplateViewModel>(deletedEmailTemplate);

            return Ok(new ResponseModel($"{CustomMessages.Deleted($"{singleEmailTemplate.Subject}")}", false, 1));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize(Roles = CustomPolicies.Administrator)]
    [HttpGet, Route("get")]
    public ActionResult Get()
    {
        try
        {
            var userId = GetUserId();

            var emailTemplates = _emailTemplateRepository.Get();

            var fvms = _mapper.Map<List<EmailTemplateViewModel>>(emailTemplates);

            return Ok(new ResponseModel($"{CustomMessages.Fetched($"{fvms.Count}", "Email Template(s)")}", false, fvms));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // TODO: Get ET by LId
    [Authorize(Roles = CustomPolicies.LIManager)]
    [HttpGet, Route("getByLIId")]
    public ActionResult GetByLIId([FromBody] CrudModel model)
    {
        try
        {
            var userId = GetUserId();

            var emailTemplates = _emailTemplateRepository.Get();

            var fvms = _mapper.Map<List<EmailTemplateViewModel>>(emailTemplates);

            return Ok(new ResponseModel($"{CustomMessages.Fetched($"{fvms.Count}", "Email Template(s)")}", false, fvms));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet, Route("getSingle")]
    public async Task<ActionResult> GetSingleIdea([FromBody] CrudModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

            var userId = GetUserId();

            var singleEmailTemplate = await _emailTemplateRepository.GetById(model.Id);
            if (singleEmailTemplate == null)
                return NotFound(new ResponseModel($"{CustomMessages.NotFound("Email Template")}", false, null));

            var fvm = _mapper.Map<EmailTemplateViewModel>(singleEmailTemplate);

            return Ok(new ResponseModel($"{CustomMessages.Fetched("1", "Email Template")}", false, 1));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // TO DO: Check if LI or Admin owns the template before update
    [Authorize(Roles = CustomPolicies.LIManager)]
    [HttpPut, Route("update")]
    public async Task<ActionResult> Update([FromBody] EmailTemplateUpdateBindingModel model)
    {
        try
        {
            var userId = GetUserId();

            var EmailTemplate = _mapper.Map<EmailTemplate>(model);
            var singleEmailTemplate = await _emailTemplateRepository.GetById(EmailTemplate.Id);
            if (singleEmailTemplate == null)
                return NotFound(new ResponseModel($"{CustomMessages.NotFound("Email Template")}", false, null));

            var updatedSingleEmailTemplate = await _emailTemplateRepository.Update(singleEmailTemplate.Id, EmailTemplate, userId);

            var fvm = _mapper.Map<EmailTemplateViewModel>(updatedSingleEmailTemplate);

            return Ok(new ResponseModel($"{CustomMessages.Updated("Email Template")}", false, fvm));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
