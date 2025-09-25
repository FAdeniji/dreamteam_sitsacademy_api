using data.models;

namespace web.apis
{
    public interface ILogRepository
    {
        IEnumerable<Log> GetLogs(DateTime start, DateTime end, string userId = "");
    }
}
