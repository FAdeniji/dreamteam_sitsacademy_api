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
    public class QuestionsController : BaseController
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public QuestionsController(IConfiguration configuration, IQuestionRepository questionRepository,
            IMapper mapper)
        {
            _configuration = configuration;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        [HttpPost, Route("add"), AllowAnonymous]
        public async Task<IActionResult> add([FromBody] QuestionBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    userId = "System";

                var question = _mapper.Map<Course>(model);
                var addedEmailTemplate = await _questionRepository.Add(question, userId);
                if (addedEmailTemplate == null)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotSaved("Question")}", false, null));

                var fvm = _mapper.Map<QuestionViewModel>(addedEmailTemplate);

                return Ok(new ResponseModel($"{CustomMessages.Saved("Question")}", false, 1));
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

                var emailTemplates = _questionRepository.Get();

                var fvms = _mapper.Map<List<QuestionViewModel>>(emailTemplates);

                return Ok(new ResponseModel($"{CustomMessages.Fetched($"{fvms.Count}", "Question(s)")}", false, fvms));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
