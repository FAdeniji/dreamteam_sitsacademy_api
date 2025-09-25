using data.models;
using ILogger = Serilog.ILogger;

namespace web.apis
{
    public class LogRepository: ILogRepository
    {
        private readonly DbConn _dbConn;
        private readonly ILogger _logger;

        public LogRepository(DbConn dbConn, ILogger logger)
        {
            _dbConn = dbConn;
            _logger = logger;
        }

        public IEnumerable<Log> GetLogs(DateTime start, DateTime end, string name)
        {
            try
            {
                var query = (from l in _dbConn.Logs
                             join u in _dbConn.Users
                             on l.UserId equals u.Id into uu
                             //from uu in users.DefaultIfEmpty()
                             select new Log()
                             {

                                 Id = l.Id,
                                 UserId = l.UserId,
                                 Type = l.Type,
                                 TableName = l.TableName,
                                 DateTime = l.DateTime,
                                 OldValues = l.OldValues,
                                 NewValues = l.NewValues,
                                 AffectedColumns = l.AffectedColumns,
                                 PrimaryKey = l.PrimaryKey,
                                 DateAdded = l.DateAdded,
                                 StaffName = $"{uu.FirstOrDefault().FirstName} {uu.FirstOrDefault().LastName}"
                             });

                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(q => q.StaffName.Contains(name));

                return query.OrderByDescending(o => o.Id)
                    .AsEnumerable();
            }

            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while fetching the logs: {ex.Message}");
                return null;
            }
        }
    }
}
