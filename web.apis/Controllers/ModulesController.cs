using AutoMapper;
using common.data;
using data.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using web.apis.Models;

namespace web.apis.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors("sitsacademy")]
    //[Authorize(Roles = CustomPolicies.Everyone)]
    public class ModulesController : BaseController
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ModulesController(IConfiguration configuration, IModuleRepository moduleRepository,
            IMapper mapper)
        {
            _configuration = configuration;
            _moduleRepository = moduleRepository;
            _mapper = mapper;
        }

        [HttpPost, Route("add"), AllowAnonymous]
        public async Task<IActionResult> add([FromBody] ModuleBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    userId = "System";

                var module = _mapper.Map<Module>(model);
                var addedEmailTemplate = await _moduleRepository.Add(module, userId);
                if (addedEmailTemplate == null)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotSaved("Module")}", false, null));

                var fvm = _mapper.Map<ModuleViewModel>(addedEmailTemplate);

                return Ok(new ResponseModel($"{CustomMessages.Saved("Module")}", false, 1));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //[Authorize(Roles = CustomPolicies.Administrator)]
        [HttpGet, Route("get"), AllowAnonymous]
        public ActionResult Get()
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    userId = "System";

                var emailTemplates = _moduleRepository.Get();

                var fvms = _mapper.Map<List<ModuleViewModel>>(emailTemplates);

                return Ok(new ResponseModel($"{CustomMessages.Fetched($"{fvms.Count}", "Question(s)")}", false, fvms));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost, Route("getbyproductcode"), AllowAnonymous]
        public ActionResult GetbyProductCode([FromBody] string productCode)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(productCode))
                    return BadRequest(new ResponseModel($"{CustomMessages.StringMessage("Course Product Code cannot be null")}", false, null));

                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    userId = "System";

                var emailTemplates = _moduleRepository.GetByProductCode(productCode);

                var fvms = _mapper.Map<List<ModuleViewModel>>(emailTemplates);

                return Ok(new ResponseModel($"{CustomMessages.Fetched($"{fvms.Count}", "Question(s)")}", false, fvms));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
