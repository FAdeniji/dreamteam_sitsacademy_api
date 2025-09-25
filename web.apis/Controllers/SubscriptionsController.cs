using common.data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using web.apis.Models;
using Microsoft.AspNetCore.Identity;
using data.models;

namespace web.apis.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors("Procent")]
    [Authorize(Roles = CustomPolicies.Everyone)]
    public class SubscriptionsController : BaseController
    {
        private readonly ISubscriptionRepository _userSubscription;
        private readonly IMapper _mapper;
        private readonly ILogger<SubscriptionsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public SubscriptionsController(ISubscriptionRepository usersSubscription, IMapper mapper,
            ILogger<SubscriptionsController> logger, UserManager<ApplicationUser> userManager)
        {
            _userSubscription = usersSubscription;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
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

                var addedFunnel = await _userSubscription.Activate(model.Id, userId);
                if (!addedFunnel)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotUpdated("Subscription")}", false, null));

                return Ok(new ResponseModel($"{CustomMessages.Updated("Subscription")}", false, 1));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet, Route("get"), AllowAnonymous]
        public ActionResult Get()
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(new ResponseModel(CustomMessages.Invalid(), false, null));

                var subscriptions = _userSubscription.Get();

                var svms = _mapper.Map<List<SubscriptionViewModel>>(subscriptions);

                return Ok(new ResponseModel($"{CustomMessages.Fetched($"{svms.Count}", "Subscription(s)")}", false, subscriptions));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost, Route("getSubscriptionByUserId")]
        public async Task<ActionResult> GetSubscriptionByUserIdAsync([FromBody] CrudModelString model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel(CustomMessages.Invalid(), false, null));

                var userSubscription = await _userSubscription.GetUserSubscription(model.Id);

                var svm = _mapper.Map<UserSubscriptionViewModel>(userSubscription);

                return Ok(new ResponseModel($"{CustomMessages.Fetched($"1", "Subscription")}", false, svm));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = CustomPolicies.Administrator)]
        [HttpDelete, Route("delete")]
        public async Task<ActionResult> Delete([FromBody] CrudModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel(CustomMessages.Invalid(), false, null));

                var userId = GetUserId();

                var subscription = await _userSubscription.GetSingle(model.Id);
                if(subscription == null)
                    return NotFound(new ResponseModel("Subscription", false, null));

                var sub = await _userSubscription.Delete(model.Id, subscription, userId);

                var svm = _mapper.Map<SubscriptionViewModel>(sub);

                return Ok(new ResponseModel($"{CustomMessages.Deleted("Subscription")}", false, svm));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = CustomPolicies.Administrator)]
        [HttpPost, Route("add")]
        public async Task<ActionResult> Add([FromBody] SubscriptionBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel(CustomMessages.Invalid(), false, null));

                var userId = GetUserId();

                var subscription = _mapper.Map<Subscription>(model);

                var addedSubscription = await _userSubscription.Add(subscription, userId);
                if (addedSubscription == null)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotSaved("Subscription")}", false, null));

                var svm = _mapper.Map<SubscriptionViewModel>(addedSubscription);

                return Ok(new ResponseModel($"{CustomMessages.Saved("Subscription")}", false, svm));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = CustomPolicies.Administrator)]
        [HttpPut, Route("update")]
        public async Task<ActionResult> Update([FromBody] SubscriptionUpdateBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel(CustomMessages.Invalid(), false, null));

                var userId = GetUserId();

                var subscription = _mapper.Map<Subscription>(model);

                var updatedSubscription = await _userSubscription.Update(subscription, userId);
                if (updatedSubscription == null)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotUpdated("Subscription")}", false, null));

                var applicationUserViewModel = _mapper.Map<SubscriptionViewModel>(updatedSubscription);

                return Ok(new ResponseModel($"{CustomMessages.Updated("Subscription")}", false, applicationUserViewModel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

