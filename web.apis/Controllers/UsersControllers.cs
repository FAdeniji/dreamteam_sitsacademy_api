using common.data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using web.apis.Models;
using Microsoft.AspNetCore.Identity;

namespace web.apis.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors("Procent")]
    [Authorize(Roles = CustomPolicies.Everyone)]
    public class UsersController : BaseController
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(IUsersRepository usersRepository, IMapper mapper,
            ILogger<UsersController> logger, UserManager<ApplicationUser> userManager)
        {
            _usersRepository = usersRepository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize(Roles = CustomPolicies.Administrator)]
        [HttpPut, Route("activate")]
        public async Task<IActionResult> Activate([FromBody] CrudModelString model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();

                var addedFunnel = await _usersRepository.Activate(model.Id, userId);
                if (!addedFunnel)
                    return BadRequest(new ResponseModel($"{CustomMessages.NotUpdated("User")}", false, null));

                return Ok(new ResponseModel($"{CustomMessages.Updated("User")}", false, 1));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = CustomPolicies.Administrator)]
        [HttpDelete, Route("delete")]
        public async Task<ActionResult> Delete([FromBody] CrudModelString model)
        {
            try
            {
                var loggedInUserId = GetUserId();
                var user = await _userManager.FindByIdAsync(loggedInUserId);
                if(user == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("User")}", false, null));

                if (user.UserRoleEnum != common.data.Enums.UserRoleEnum.Administrator)
                {
                    if(loggedInUserId != model.Id)
                        return BadRequest(new ResponseModel("User does not own profile", false, null));
                }

                var singleUser = await _usersRepository.GetSingle(model.Id);
                if (singleUser == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("User")}", false, null));

                var deletedRes = await _usersRepository.Delete(loggedInUserId, singleUser.Id);

                return Ok(new ResponseModel($"{CustomMessages.Deleted($"{singleUser.FirstName}")}", false, deletedRes));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = CustomPolicies.Administrator)]
        [HttpGet, Route("getActiveUserTypes")]
        public ActionResult GetActiveUserTypes()
        {
            try
            {
                var applicationUsers = _usersRepository.GetActiveUserTypes();

                return Ok(new ResponseModel($"{CustomMessages.Fetched($"{applicationUsers.Count()}", "Active User Types")}", false, applicationUsers));
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
                var applicationUsers = _usersRepository.Get();

                return Ok(new ResponseModel($"{CustomMessages.Fetched($"{applicationUsers.Count()}", "Users")}", false, applicationUsers));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost, Route("getSingle")]
        public async Task<ActionResult> GetSingle([FromBody] CrudModelString model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();
                var loggedInUser = await _userManager.FindByIdAsync(userId);

                if(loggedInUser == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("User")}", false, null));

                if(!await _userManager.IsInRoleAsync(loggedInUser, CustomPolicies.Administrator.ToString()))
                {
                    if(model.Id != userId)
                        return BadRequest(new ResponseModel($"{CustomMessages.StringMessage("Cannot access another user's account")}", false, null));
                }

                var user = await _usersRepository.GetSingle(model.Id);
                if (user == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("User")}", false, null));

                user.Documents = user.Documents?.Where(u => u.TypeId == DocumentType.ProfilePicture).OrderByDescending(d => d.Id).Take(1).ToList();

                var applicationUserViewModel = _mapper.Map<UserViewModel>(user);

                user.LastName = Security.AnonymiseData(user.LastName);
                user.Email = Security.Anonymise(user.Email);
                user.PhoneNumber = Security.AnonymiseNumber(user.PhoneNumber.Substring(0, 2));

                return Ok(new ResponseModel($"{CustomMessages.Fetched("1", "User")}", false, applicationUserViewModel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

