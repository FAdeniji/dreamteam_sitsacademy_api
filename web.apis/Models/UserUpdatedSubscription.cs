namespace web.apis
{
    public class UserUpdatedSubscription
	{
		public UserUpdatedSubscription(string userId, string loggedInUserId, int subscriptionId)
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

