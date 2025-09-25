using data.models;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace web.apis
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DbConn _dbConn;
        private readonly ILogger _logger;

        public NotificationRepository(DbConn dbConn, ILogger logger)
        {
            _dbConn = dbConn;
            _logger = logger;
        }

        public async Task<Notification> Add(Notification notification, string userId)
        {
            try
            {
                await _dbConn.Notifications.AddAsync(notification);
                await _dbConn.SaveChangesAsync(userId);

                return notification;
            }
            catch (Exception ex)
            {
                _logger.Error("Error adding notification", ex);
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Notification> Get(string userId)
        {
            try
            {
                return _dbConn.Notifications
                        .AsNoTracking()
                        .Where(n => n.UserId == userId)
                        .OrderByDescending(n => n.Id)
                        .AsEnumerable();
                
            }
            catch (Exception ex)
            {
                _logger.Error("Error getting notification", ex);
                throw new Exception(ex.Message);
            }
        }
    }
}

