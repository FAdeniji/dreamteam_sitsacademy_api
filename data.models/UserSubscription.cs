using System.ComponentModel.DataAnnotations;
using common.data;

namespace data.models
{
    public class UserSubscription : Entity
    {
        public UserSubscription()
        {
            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }

        public UserSubscription(int subscriptionId, string userId, DateTime expiryDate, double price)
        {
            UserId = userId;
            SubscriptionId = subscriptionId;
            ExpiryDate = expiryDate;
            Price = price;

            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }

        [Required]
        public int SubscriptionId { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime ExpiryDate { get; set; }

        [Required]
        public double Price { get; set; }

        public string? TransactionRef { get; set; }

        public virtual Subscription? Subscription { get; set; }
    }
}

