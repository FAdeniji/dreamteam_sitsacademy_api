using data.models;

namespace web.apis
{
    public interface ISubscriptionRepository
	{
        IEnumerable<Subscription> Get();
        Task<Subscription> Add(Subscription subscription, string userId);
        Task<UserSubscription> AddUserSubscription(Subscription subscription, string userId, string loggedInUserId);
        Task<Subscription> Delete(int id, Subscription subscription, string userId);
        Task<Subscription> Update(Subscription subscription, string userId);
        Task<Subscription> GetSingle(int id);
        Task<Subscription> GetLISubscription();
        Task<Subscription> GetInvestorSubscription();
        Task<UserSubscription> GetUserSubscription(string userId, bool ant = true);
        Task<UserSubscription> UpdateUserSubscription(Subscription subscription, string userId, string loggedInUserId);
        Task<bool> Activate(int id, string userId);
        Task<int> GetNoIfIdeasForSubscriptionByTopic(string name);

    }
}

