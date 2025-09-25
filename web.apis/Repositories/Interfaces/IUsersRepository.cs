using data.models;
using web.apis.Models;

namespace web.apis
{
    public interface IUsersRepository
    {
        IEnumerable<ApplicationUserViewModel> Get();
        IEnumerable<object> GetActiveUserTypes();
        Task<ApplicationUser> GetSingle(string userId);
        Task<bool> Delete(string loggedInUserId, string userId);
        Task<bool> Activate(string id, string userId);
    }
}

