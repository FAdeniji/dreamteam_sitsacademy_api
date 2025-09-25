
using AutoMapper;
using common.data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using web.apis.Models;
using web.apis.ViewModels;
using ILogger = Serilog.ILogger;

namespace web.apis.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors("Procent")]
    [Authorize(Roles = CustomPolicies.Everyone)]
    public class NotificationsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly INotificationRepository _notificationRepository;

        public NotificationsController(IMapper mapper, ILogger logger, INotificationRepository notificationRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _notificationRepository = notificationRepository;
        }

        [HttpGet, Route("get")]
        public ActionResult Get()
        {
            try
            {
                var userId = GetUserId();

                var lstNotifications = _notificationRepository.Get(userId).ToList();

                var notificationViewModels = _mapper.Map<List<NotificationViewModel>>(lstNotifications);

                return Ok(new ResponseModel($"{CustomMessages.Fetched($"{notificationViewModels.Count}", "Notifications")}", false, notificationViewModels));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}