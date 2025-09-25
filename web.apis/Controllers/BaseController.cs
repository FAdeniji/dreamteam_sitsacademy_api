using common.data.Enums;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace web.apis.Controllers
{
    /// <summary>
    /// BaseController
    /// </summary>
    [EnableCors("sitsacademy")]
    public class BaseController : Controller
    {
        protected string GetUserId()
        {
            string result;
            var claim = this.User?.Claims?.FirstOrDefault(i => i.Type == "userId");
            if (claim == null)
            {
                result = null;
            }
            else
            {
                result = claim.Value;
            }

            return result;
        }

        protected string GetRole()
        {
            var prop = this.User?.Claims?.Select(x => x.Value).ToArray();
            if (prop != null && prop.Length > 2)
            {
                return prop[2];
            }

            return "User";
        }

        protected string GetUserName()
        {
            var prop = this.User?.Claims?.Select(x => x.Value).ToArray();
            if (prop != null && prop.Length > 2)
            {
                return prop[1];
            }

            return "UserName";
        }

        protected string GetEmailAddress()
        {
            var prop = this.User?.Claims?.Select(x => x.Value).ToArray();
            if (prop != null && prop.Length > 2)
            {
                return prop[3];
            }

            return "Student";
        }

        protected UserRoleEnum GetAccountType()
        {
            return (UserRoleEnum)Enum.Parse(typeof(UserRoleEnum), GetRole());
        }

        protected bool IsInRole(UserRoleEnum userType)
        {
            if (GetAccountType() == userType)
            {
                return true;
            }

            return false;
        }
    }
}

