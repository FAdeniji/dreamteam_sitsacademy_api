using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using common.data;
using common.data.Enums;
using common.data.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using web.apis.BindingModels;
using web.apis.Models;
using web.apis.Queries;
using web.apis.ViewModels;

namespace web.apis.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors("sitsacademy")]
    public class AccountsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly ISender _mediator;
        private readonly IEmailRepository _emailRepository;
        public AccountsController(IMapper mapper, UserManager<ApplicationUser> userManager,
            IConfiguration configuration, RoleManager<IdentityRole> roleManager,
            ILogger<AccountsController> logger, IMediator mediator, IEmailRepository emailRepository)
        {
            _mapper = mapper;
            _configuration = configuration;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _mediator = mediator;
            _emailRepository = emailRepository;
        }

        [Authorize(Roles = CustomPolicies.Everyone)]
        [HttpGet, Route("getUserTypes")]
        public IActionResult UserTypes()
        {
            var userTypes = Enum<UserRoleEnum>.GetAllValuesAsIEnumerable().Select(d => new EnumDTO(d));
            return Ok(userTypes);
        }

        [HttpGet, Route("getAPIBaseUrl"), AllowAnonymous]
        public IActionResult GetAPIBaseUrl()
        {
            return Ok(new { 
                devAPIUrl = _configuration.GetValue<string>("Urls:DevBaseUrl"), 
                prodAPIUrl = _configuration.GetValue<string>("Urls:ProdBaseUrl") 
            });
        }

        [Authorize(Roles = CustomPolicies.LIAdmin)]
        [HttpPost, Route("generateInvitationLink")]
        public async Task<IActionResult> generateInvitationLink([FromBody] CrudModelString model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel(CustomMessages.Invalid(), true, JsonConvert.SerializeObject(model)));

                var user = await _userManager.Users
                    .AsNoTracking()
                    .Where(u => u.Id == model.Id)
                    .FirstOrDefaultAsync();

                if (user == null)
                    return NotFound(new ResponseModel("User not found", false, null));

                var sender = $"{user.Id}|{user.UserRoleEnum.GetDisplayName()}";
                var inviteLink = _configuration.GetValue<string>("Urls:InviteLink");
                var res = await _emailRepository.SendEmail(user.FirstName, user.Email, "invite_link", keyValuePairs: new Dictionary<string, string>() { { "invitationlink", $"{inviteLink}?sender={sender}" } });
                if (!res)
                {
                    return BadRequest(new ResponseModel("Invitation link not sent", true, null));
                }

                return Ok(new ResponseModel("Invitation link sent successfully", true, null));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost, Route("reset"), AllowAnonymous]
        public async Task<IActionResult> ResetPasswordByEmail([FromBody] ResetPasswordByEmail model)
        {
            if(!ModelState.IsValid)
                return BadRequest(new ResponseModel(CustomMessages.Invalid(), true, JsonConvert.SerializeObject(model)));

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
                return BadRequest(new ResponseModel("Invalid credentials", true, JsonConvert.SerializeObject(model)));

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, "https");

            // send email with callbackurl
            return Ok(new ResponseModel($"Password reset email sent to registered email address", false, null));
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginBindingModel model)
        {
            try
            {
                // check if model is valid
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel(CustomMessages.Invalid(), true, JsonConvert.SerializeObject(model)));

                // check if user exists
                var user = await _userManager.Users
                    .Where(u => u.Email == model.Email)
                    .FirstOrDefaultAsync();

                if (user == null)
                    return BadRequest(new ResponseModel("Invalid credentials", true, JsonConvert.SerializeObject(model)));
                
                // check if user has confirmed email
                if (!user.EmailConfirmed)
                    return BadRequest(new ResponseModel("User has not confirmed email address", true, JsonConvert.SerializeObject(model)));

                if (!await _userManager.CheckPasswordAsync(user, model.Password))
                    return BadRequest(new ResponseModel("Invalid user credentials", true, JsonConvert.SerializeObject(model)));

                var token = GenerateJWT(user);

                var userViewModel = _mapper.Map<UserViewModel>(user);

                string message = string.Empty;
                switch (user.UserRoleEnum)
                {
                    // case UserRoleEnum.LearningInstitution:
                    // 
                    //     break;
                    // case UserRoleEnum.Lecturer:
                    //     break;
                    // case UserRoleEnum.Student:
                    //     break;
                    default:
                        message = "None";
                        break;
                }

                return Ok(new ResponseModel("Login was successful", false, new
                {
                    Token = token,
                    User = userViewModel,
                    Message = message
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost, Route("register"), AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessage = new List<string>();
                    foreach (var modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            errorMessage.Add(error.ErrorMessage);
                        }
                    }
                    return BadRequest(new ResponseModel(CustomMessages.Invalid(), true, string.Join(", ", errorMessage)));
                }

                var loggedInUserId = GetUserId();
                var loggedInUser = await _userManager.FindByIdAsync(loggedInUserId);
                if(loggedInUser != null && loggedInUser.UserRoleEnum == UserRoleEnum.Administrator)
                {
                    if (string.IsNullOrWhiteSpace(model.UserRoleEnum))
                    {
                        return BadRequest(new ResponseModel(CustomMessages.StringMessage("Please provide the user's role"), true, null));
                    }
                }
                else
                {
                    model.UserRoleEnum = UserRoleEnum.User.GetDisplayName();
                }

                var user = _mapper.Map<ApplicationUser>(model);
                user.IsActive = true;

                var response = new Response<ApplicationUser>(user);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    var errorMessage = new string[result.Errors.Count()];
                    for (var i = 0; i < result.Errors.Count(); i++)
                    {
                        errorMessage[i] = $"{result.Errors.ElementAt(i).Code}, {result.Errors.ElementAt(i).Description}";
                    }

                    response.Data = null;
                    response.Message = "An error occurred while creating the account";

                    response.Succeeded = false;
                    response.Errors = errorMessage;

                    return BadRequest(new ResponseModel(string.Join(",", errorMessage.ToList()), true, response));
                }

                // add user to role
                var roleName = string.Empty;
                var userType = Enum.Parse<UserRoleEnum>(model.UserRoleEnum);
                switch (userType)
                {
                    // case UserRoleEnum.IdeaOwner:
                    //     roleName = UserRoleEnum.IdeaOwner.GetDisplayName();
                    //     user.UserRoleEnum = UserRoleEnum.IdeaOwner;
                    // 
                    //     break;
                    // case UserRoleEnum.CoIdeaOwner:
                    //     roleName = UserRoleEnum.CoIdeaOwner.GetDisplayName();
                    //     user.UserRoleEnum = UserRoleEnum.CoIdeaOwner;
                    // 
                    //     break;
                    // case UserRoleEnum.Investor:
                    //     roleName = UserRoleEnum.Investor.GetDisplayName();
                    //     user.UserRoleEnum = UserRoleEnum.Investor;
                    // 
                    //     break;
                    // case UserRoleEnum.Institution:
                    //     break;
                    // case UserRoleEnum.Administrator:
                    //     roleName = UserRoleEnum.Administrator.GetDisplayName();
                    //     user.UserRoleEnum = UserRoleEnum.Administrator;
                    // 
                    //     break;
                    // case UserRoleEnum.LearningInstitution:
                    //     roleName = UserRoleEnum.LearningInstitution.GetDisplayName();
                    //     user.UserRoleEnum = UserRoleEnum.LearningInstitution;
                    // 
                    //     break;
                    // case UserRoleEnum.Student:
                    //     roleName = UserRoleEnum.Student.GetDisplayName();
                    //     user.UserRoleEnum = UserRoleEnum.Student;
                    // 
                    //     break;
                    // case UserRoleEnum.Lecturer:
                    //     roleName = UserRoleEnum.Lecturer.GetDisplayName();
                    //     user.UserRoleEnum = UserRoleEnum.Lecturer;

                    //     break;
                    default:
                        user.UserRoleEnum = UserRoleEnum.User;
                        break;
                }

                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // create role
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                var roleToUser = await _userManager.AddToRoleAsync(user, roleName);
                if (roleToUser.Succeeded)
                    response.Message = $"User account was successfully created for {user.FirstName}";

                var userViewModel = _mapper.Map<UserViewModel>(user);

                switch (userType)
                {
                    // case UserRoleEnum.IdeaOwner:
                    //     roleName = UserRoleEnum.IdeaOwner.GetDisplayName();
                    // 
                    //     break;
                    // case UserRoleEnum.CoIdeaOwner:
                    //     break;
                    // case UserRoleEnum.Investor:
                    //     // Add to Funnel and log reminder
                    //     
                    //     break;
                    // case UserRoleEnum.Institution:
                    //     break;
                    // case UserRoleEnum.Administrator:
                    //     break;
                    // case UserRoleEnum.LearningInstitution:
                    // 
                    //     break;
                    default:
                        break;
                }

                return Ok(new ResponseModel($"User account was successfully created.", false, userViewModel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost, Route("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessage = new List<string>();
                    foreach (var modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            errorMessage.Add(error.ErrorMessage);
                        }
                    }
                    return BadRequest(new ResponseModel(CustomMessages.Invalid(), true, string.Join(", ", errorMessage)));
                }

                var user = await _userManager.Users.Where(u => u.Email == model.Email
                    || u.NormalizedEmail == model.Email).FirstOrDefaultAsync();

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return BadRequest(new ResponseModel("Please check the email you used to create your account", false, null));
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = System.Web.HttpUtility.UrlEncode(code);
                var url = _configuration.GetValue<string>("Urls:passwordreset");
                var res = await _emailRepository.SendEmail(user.FirstName, user.Email, "forgot_password", $"{url}?code={code}&email={user.Email}");
                if (!res)
                {
                    return BadRequest(new ResponseModel("Password reset email not sent", true, null));
                }

                return Ok(new ResponseModel("Password request sent successfully", true, null));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost, Route("userResetPassword")]
        public async Task<IActionResult> UserResetPassword([FromBody] UserResetPasswordBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel(CustomMessages.Invalid(), false, null));

                var userId = GetUserId();

                var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var resetPasswordResult = await _userManager.ResetPasswordAsync(user, code, model.Password);
                if (resetPasswordResult.Succeeded)
                {
                    return Ok(new ResponseModel("Password updated successfully", true, null));
                }
                else
                {
                    return BadRequest(new ResponseModel("Password was NOT updated", true, null));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost, Route("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel(CustomMessages.Invalid(), false, null));

                var user = await _userManager.Users.Where(u => u.Email == model.Email
                    || u.NormalizedEmail == model.Email).FirstOrDefaultAsync();
                if (user == null)
                    return NotFound(new ResponseModel("User not found", false, null));

                var code = System.Web.HttpUtility.UrlDecode(model.Code);

                var resetPasswordResult = await _userManager.ResetPasswordAsync(user, code, model.Password);
                if (resetPasswordResult.Succeeded)
                {
                    return Ok(new ResponseModel("Password updated successfully", true, null));
                }
                else
                {
                    return BadRequest(new ResponseModel("Password was NOT updated", true, null));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut, Route("updateUser")]
        public async Task<ActionResult> UpdateUser([FromBody] UserRegistrationUpdateBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel($"{CustomMessages.Invalid()}", false, null));

                var userId = GetUserId();

                var user = await _userManager.FindByIdAsync(model.UserId);

                if (user == null)
                    return NotFound(new ResponseModel($"{CustomMessages.NotFound("User")}", false, null));

                if (user.FirstName != model.FirstName)
                    user.FirstName = model.FirstName;

                if (user.LastName != model.LastName)
                    user.LastName = model.LastName;

                if (user.Email != model.EmailAddress)
                    user.Email = model.EmailAddress;

                if (user.Department != model.Department)
                    user.Department = model.Department;

                await _userManager.UpdateAsync(user);

                return Ok(new ResponseModel($"{CustomMessages.Updated("User")}", false, user));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet, Route("getPasswordPolicy"), AllowAnonymous]
        public ActionResult GetPasswordPolicy()
        {
            try
            {
                var passwordPolicyViewModel = new PasswordPolicyViewModel()
                {
                    RequireDigit = _configuration.GetValue<bool>("PasswordConfig:RequireDigit"),
                    RequireLowercase = _configuration.GetValue<bool>("PasswordConfig:RequireLowercase"),
                    RequireNonAlphanumeric = _configuration.GetValue<bool>("PasswordConfig:RequireNonAlphanumeric"),
                    RequiredLength = Convert.ToInt32(_configuration.GetValue<string>("PasswordConfig:PasswordRequiredLength")),
                    RequireUppercase = _configuration.GetValue<bool>("PasswordConfig:RequireUppercase"),
                    RequiredUniqueChars = Convert.ToInt32(_configuration.GetValue<string>("PasswordConfig:NoOfUniqueChar")),
                    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(_configuration.GetValue<string>("LockOut:LockoutMins"))),
                    MaxFailedAccessAttempts = Convert.ToInt32(_configuration.GetValue<string>("LockOut:LockOutAttempts")),
                    AllowedForNewUsers = _configuration.GetValue<bool>("PasswordConfig:RequireNonAlphanumeric"),
                    AllowedUserNameCharacters = _configuration.GetValue<string>("PasswordConfig:AllowedUserNameCharacters")
                };      

                return Ok(new ResponseModel($"{CustomMessages.Fetched("", "Password Policy")}", false, passwordPolicyViewModel));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = CustomPolicies.Everyone)]
        [HttpPost, Route("updateUserRole")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserBingingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseModel(CustomMessages.Invalid(), true, null));
                }

                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound(new ResponseModel("User not found", false, null));
                }

                // Add to Funnel
                user.UserRoleEnum = model.UserRoleEnum;
                await _userManager.UpdateAsync(user);

                // Add user to role
                await _userManager.AddToRoleAsync(user, model.UserRoleEnum.GetDisplayName());

                return Ok(new ResponseModel($"User account was successfully updated", false, new ApplicationUser()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserRoleEnum = user.UserRoleEnum
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #region Generate JWT
        private string GenerateJWT(ApplicationUser user)
        {
            var audience = _configuration.GetValue<string>("JWT:Audience");
            var issuer = _configuration.GetValue<string>("JWT:Issuer");
            var secret = _configuration.GetValue<string>("JWT:Secret");
            var expiration = _configuration.GetValue<int>("JWT:Expiration");

            var claims = new List<Claim>
            {
                new Claim("userId", user.Id),
                new Claim("userName", user.UserName),
                new Claim("department", user.Department),
                new Claim("role", user.UserRoleEnum.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var expires = DateTime.Now.AddHours(Convert.ToDouble(expiration));

            var token = new JwtSecurityToken(issuer, audience, claims, DateTime.Now, expires, creds);            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}