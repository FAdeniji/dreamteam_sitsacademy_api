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
    public class CampaignsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ICampaignRepository _campaignRepository;
        private readonly IPromoCodeRepository _promoCodeRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public CampaignsController(IMapper mapper, ICampaignRepository campaignRepository,
           ILogger logger, IConfiguration configuration, UserManager<ApplicationUser> userManager,
           IPromoCodeRepository promoCodeRepository)
        {
            _mapper = mapper;
            _campaignRepository = campaignRepository;
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
            _promoCodeRepository = promoCodeRepository;
        }

        [Authorize(Roles = CustomPolicies.LIManager)]
        [HttpPost, Route("add")]
        public async Task<IActionResult> add([FromBody] CampaignBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();

                var campaign = _mapper.Map<Campaign>(model);

                campaign.CreatorUserId = userId;

                var promocodes = _promoCodeRepository.GetList(model.PromoCodeIds, userId);
                foreach (var promoCode in promocodes)
                {
                    campaign.AddPromoCodes(promoCode);
                }

                var addedCampaign = await _campaignRepository.Add(campaign, userId);
                if (addedCampaign == null)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotSaved("Campaign")}", false, null));

                var pvm = _mapper.Map<CampaignViewModel>(addedCampaign);

                var username = await _userManager.FindByIdAsync(userId);

                pvm.CreatorName = $"{username?.FirstName} {username?.LastName}";

                return Ok(new ResponseModel($"{CustomMessages.Saved("Campaign")}", false, pvm));
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

                var addedEmailTemplate = await _campaignRepository.Activate(model.Id, userId);
                if (!addedEmailTemplate)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotUpdated("Campaign")}", false, null));

                return Ok(new ResponseModel($"{CustomMessages.Updated("Campaign")}", false, 1));
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
                var deletedDocument = await _campaignRepository.GetSingle(model.Id, userId);
                if (deletedDocument == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("Campaign")}", false, null));

                deletedDocument = await _campaignRepository.Delete(deletedDocument.Id, userId);
                if (deletedDocument == null)
                    return BadRequest(new ResponseModel($"{CustomMessages.StringMessage("Unable to delete document")}", false, null));

                var pvm = _mapper.Map<CampaignViewModel>(deletedDocument);

                var username = await _userManager.FindByIdAsync(userId);

                pvm.CreatorName = $"{username?.FirstName} {username?.LastName}";

                return Ok(new ResponseModel($"{CustomMessages.Deleted("Campaign")}", false, deletedDocument));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet, Route("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = GetUserId();
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("User")}", false, null));

                var campaigns = new List<Campaign>();
                if (user.UserRoleEnum == UserRoleEnum.Administrator)
                    campaigns = _campaignRepository.GetAll().ToList();
                else
                    campaigns = _campaignRepository.Get(userId).ToList();
                
                var ivms = _mapper.Map<List<CampaignViewModel>>(campaigns);

                foreach (var ivm in ivms)
                {
                    var campaign = campaigns.Where(p => p.Id == ivm.Id).FirstOrDefault();

                    var username = await _userManager.FindByIdAsync(campaign?.CreatorUserId);

                    ivm.CreatorName = $"{username?.FirstName} {username?.LastName}";
                }

                return Ok(new ResponseModel($"{CustomMessages.Fetched(ivms.Count().ToString(), "Campaign")}", false, ivms));
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

                var Campaign = await _campaignRepository.GetSingle(model.Id, userId);
                if (Campaign == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("Campaign")}", false, null));

                var pvm = _mapper.Map<CampaignViewModel>(Campaign);

                var username = await _userManager.FindByIdAsync(userId);

                pvm.CreatorName = $"{username?.FirstName} {username?.LastName}";

                return Ok(new ResponseModel($"{CustomMessages.Fetched("1", "Campaign")}", false, pvm));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut, Route("update")]
        public async Task<IActionResult> Update([FromBody] CampaignUpdateBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();

                var singleCampaign = await _campaignRepository.GetSingle(model.Id, userId);
                if (singleCampaign == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("Campaign")}", false, null));

                var campaign = _mapper.Map<Campaign>(model);

                foreach (var promoCode in campaign.PromoCodes)
                {
                    campaign.RemovePromoCodes(promoCode);
                }

                var promocodes = _promoCodeRepository.GetList(model.PromoCodeIds, userId);
                foreach (var promoCode in promocodes)
                {
                    campaign.AddPromoCodes(promoCode);
                }

                var updatedDocument = await _campaignRepository.Update(campaign.Id, campaign, userId);
                if (updatedDocument == null)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotUpdated("Campaign")}", false, null));

                var pvm = _mapper.Map<CampaignViewModel>(updatedDocument);

                var username = await _userManager.FindByIdAsync(userId);

                pvm.CreatorName = $"{username?.FirstName} {username?.LastName}";

                return Ok(new ResponseModel($"{CustomMessages.Updated("Campaign")}", false, pvm));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}