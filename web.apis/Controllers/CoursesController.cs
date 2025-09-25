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
    [Authorize(Roles = CustomPolicies.Everyone)]
    public class CoursesController : BaseController
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CoursesController(IConfiguration configuration, ICourseRepository courseRepository,
            IMapper mapper)
        {
            _configuration = configuration;
            _courseRepository = courseRepository;
            _mapper = mapper;
        }


        [HttpPost, Route("add"), AllowAnonymous]
        public async Task<IActionResult> add([FromBody] EmailTemplateBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();

                var course = _mapper.Map<Course>(model);
                var addedEmailTemplate = await _courseRepository.Add(course, userId);
                if (addedEmailTemplate == null)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotSaved("Course")}", false, null));

                var fvm = _mapper.Map<CourseViewModel>(addedEmailTemplate);

                return Ok(new ResponseModel($"{CustomMessages.Saved("Course")}", false, 1));
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

                var emailTemplates = _courseRepository.Get();

                var fvms = _mapper.Map<List<CourseViewModel>>(emailTemplates);

                return Ok(new ResponseModel($"{CustomMessages.Fetched($"{fvms.Count}", "Course(s)")}", false, fvms));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
