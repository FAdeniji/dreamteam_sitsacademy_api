using System.ComponentModel.DataAnnotations;
using common.data;

namespace data.models
{
    public class Transaction : Entity
	{
		public Transaction()
		{
            IsActive = true;
            DateAdded = DateTime.UtcNow;
		}

        public Transaction(string transactionRef, decimal amount, string naration, string userId, string paymentRequestAsJson, string paymentResponseAsJson, int subscriptionId, ProductType productType, string customerName, string customerEmail)
        {
            if (string.IsNullOrWhiteSpace(transactionRef))
            {
                throw new Exception("Transaction reference must be supplied");
            }

            TransationRef = transactionRef;
            Amount = amount;
            Naration = naration;
            UserId = userId;
            SubscriptionId = subscriptionId;

            PaymentRequestAsJson = paymentRequestAsJson;
            PaymentResponseAsJson = paymentResponseAsJson;
            IsActive = true;
            DateAdded = DateTime.UtcNow;
            ProductType = productType;
            CustomerName = customerName;
            CustomerEmail = customerEmail;

            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string TransationRef { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Naration { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string CustomerEmail { get; set; }

        public int? SubscriptionId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public ProductType ProductType { get; set; }

        [Required]
        public string PaymentRequestAsJson { get; set; }

        [Required]
        public string PaymentResponseAsJson { get; set; }

        public virtual Subscription? Subscription { get; set; }
    }
}