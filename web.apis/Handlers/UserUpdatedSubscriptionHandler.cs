using MediatR;
using ILogger = Serilog.ILogger;

namespace web.apis
{
    public class UserUpdatedSubscriptionHandler : IRequestHandler<UserUpdatedSubscriptionQuery, UserUpdatedSubscription>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ILogger _logger;

        public UserUpdatedSubscriptionHandler(ILogger logger, ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _logger = logger;
        }

        public async Task<UserUpdatedSubscription> Handle(UserUpdatedSubscriptionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = request.UserId;
                var loggedInUserId = request.LoggedInUserId;
                var subscriptionId = request.SubscriptionId;

                // get user subscription
                var userSubscription = await _subscriptionRepository.GetUserSubscription(userId, false);
                if (userSubscription == null)
                    throw new Exception("User subscription not found");

                var currentSubscription = userSubscription.Subscription;

                // get new sub
                var newSubscription = await _subscriptionRepository.GetSingle(subscriptionId);

                // update user's sub
                userSubscription.SubscriptionId = subscriptionId;
                userSubscription.Subscription = newSubscription;

                var res = await _subscriptionRepository.UpdateUserSubscription(newSubscription, userId, loggedInUserId);
                if (res == null)
                    _logger.Error($"Unable to update the subscription to {newSubscription.Topic}");
                else
                    _logger.Information($"Updated the subscription to {newSubscription.Topic}");

                return new UserUpdatedSubscription(request.UserId, request.LoggedInUserId, request.SubscriptionId);
            }
            catch (Exception ex)
            {
                _logger.Error("An error occured while saving funnel info and ideaquestions", ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}

