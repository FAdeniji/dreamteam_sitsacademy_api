using common.data;
using data.models;
using Microsoft.EntityFrameworkCore;
using web.apis;
using web.apis.Models;
using ILogger = Serilog.ILogger;

public class UsersRepository : IUsersRepository
{
    private readonly DbConn _dbConn;
    private readonly ILogger _logger;

    public UsersRepository(DbConn dbConn, ILogger logger)
    {
        _dbConn = dbConn;
        _logger = logger;
    }

    public async Task<bool> Activate(string id, string userId)
    {
        try
        {
            var userToBeDeleted = await _dbConn.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (userToBeDeleted == null)
                return false;

            userToBeDeleted.IsActive = true;
            await _dbConn.SaveChangesAsync(userId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error("Error activating user", ex);
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> Delete(string loggedInUserId, string userId)
    {
        try
        {
            var userToBeDeleted = await _dbConn.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (userToBeDeleted == null)
                return false;

            userToBeDeleted.IsActive = false;
            await _dbConn.SaveChangesAsync(loggedInUserId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error("Error deleting user", ex);
            throw new Exception(ex.Message);
        }
    }

    public IEnumerable<ApplicationUserViewModel> Get()
    {
        try
        {
            var query = from u in _dbConn.Users.Include(u => u.Documents)
                        select u;

            return query
                .OrderByDescending(u => u.DateAdded)
                .Select(x => new ApplicationUserViewModel()
                {
                    Id = x.Id,
                    OrganisationName = x.OrganisationName,
                    FirstName = x.FirstName,
                    LastName = Security.AnonymiseData(x.LastName),
                    EmailAddress = Security.Anonymise(x.Email),
                    MobileNumber = Security.AnonymiseNumber(x.PhoneNumber.Substring(0, 2)),
                    UserRoleEnum = x.UserRoleEnum.ToString(),
                    IsActive = x.IsActive,
                    Website = x.Website,
                    DateAdded = x.DateAdded
                })
                .AsEnumerable();
        }
        catch (Exception ex)
        {
            _logger.Error("Error getting all users", ex);
            throw new Exception(ex.Message);
        }
    }

    public IEnumerable<object> GetActiveUserTypes()
    {
        try
        {
            return _dbConn.Roles.Select(x => new {
                RoleName = x.Name,
                Id = x.Id
            }).AsEnumerable();
        }
        catch (Exception ex)
        {
            _logger.Error("Error getting all user types", ex);
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApplicationUser> GetSingle(string userId)
    {
        try
        {
            return await _dbConn.Users
                .Include(u => u.Documents)
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.Error("Error getting single user", ex);
            throw new Exception(ex.Message);
        }
    }
}