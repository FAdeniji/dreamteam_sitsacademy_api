using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using common.data;
using AutoMapper;
using web.apis.Models;
using Microsoft.AspNetCore.Identity;

namespace web.apis.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors("sitsacademy")]
    [Authorize(Roles = CustomPolicies.Everyone)]
    public class DashboardController : BaseController
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DashboardController> _logger;
        private UserManager<ApplicationUser> _userManager;

        public DashboardController(IDashboardRepository dashboardRepository, IMapper mapper,
            ILogger<DashboardController> logger, UserManager<ApplicationUser> userManager)
        {
            _dashboardRepository = dashboardRepository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet, Route("get")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var userId = GetUserId();
                var user = await _userManager.FindByIdAsync(userId);

                var totalUsers = _dashboardRepository.TotalUsers(user.LearningInstitutionId);               
                
                return Ok(new ResponseModel($"{CustomMessages.Fetched("", "Dashboard Data")}", false, null));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

