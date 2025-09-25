using System.ComponentModel.DataAnnotations;

namespace web.apis
{
    public class SubscriptionViewModel
	{
        [Required]
        public int Id { get; set; }

        [Required]
        public string Topic { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string ExpiryInMonths { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        public bool IsActive { get; set; }

        public string? ColourCode { get; set; }

        [Required]
        public int NoOfIdeas { get; set; }
    }

    public class UserSubscriptionViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int SubscriptionId { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime ExpiryDate { get; set; }

        [Required]
        public double Price { get; set; }

        public string? TransactionRef { get; set; }

        public virtual SubscriptionViewModel? Subscription { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        public bool IsActive { get; set; }
    }
}

