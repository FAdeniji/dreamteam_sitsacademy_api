using data.models;

namespace web.apis
{
    public interface INotificationRepository
	{
		Task<Notification> Add(Notification notification, string userId);
		IEnumerable<Notification> Get(string userId);
	}
}

