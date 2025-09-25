using Microsoft.EntityFrameworkCore;
using web.apis.Models;
using ILogger = Serilog.ILogger;

namespace web.apis
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DbConn _dbConn;
        private readonly ILogger _logger;

        public DashboardRepository(DbConn dbConn, ILogger logger)
		{
            _dbConn = dbConn;
            _logger = logger;
        }

        public IEnumerable<CategoriesAndNoOfIdeas> CategoriesAndNoOfIdeas()
        {
            try
            {
               
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error("Error getting total categories and ideas", ex);
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<ApplicationUser> TotalUsers(int? learningInstitutionId)
        {
            try
            {
                var query = _dbConn.Users
                    .AsNoTracking()
                    .AsEnumerable();

                if (learningInstitutionId.HasValue)
                    query = query.Where(u => u.LearningInstitutionId == learningInstitutionId);

                return query;
            }
            catch (Exception ex)
            {
                _logger.Error("Error getting total users", ex);
                throw new Exception(ex.Message);
            }
        }
    }
}

