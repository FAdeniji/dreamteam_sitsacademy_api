using AutoMapper;
using common.data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using web.apis.Models;

namespace web.apis.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors("Procent")]
    [Authorize(Roles = CustomPolicies.Everyone)]
    public class EmailsController : BaseController
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IMapper _mapper;

        public EmailsController(IEmailRepository emailRepository, IMapper mapper)
        {
            _emailRepository = emailRepository;
            _mapper = mapper;
        }

        [HttpGet, Route("triggerEmailSending")]
        public async Task<IActionResult> SendEmail()
        {
            var emailsSent = await _emailRepository.SendEmails();

            return Ok(new ResponseModel($"{emailsSent} email(s) sent", false, null));
        }
    }
}

