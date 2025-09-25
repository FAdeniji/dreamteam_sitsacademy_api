using MediatR;

namespace web.apis
{
    public class UserUpdatedSubscriptionQuery : IRequest<UserUpdatedSubscription>
    {
        public UserUpdatedSubscriptionQuery(string userId, string loggedInUserId, int subscriptionId)
        {
            UserId = userId;
            LoggedInUserId = loggedInUserId;
            SubscriptionId = subscriptionId;
        }

        public string UserId { get; set; }

        public string LoggedInUserId { get; set; }

        public int SubscriptionId { get; set; }
    }
}

