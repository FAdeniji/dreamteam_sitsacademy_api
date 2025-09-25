using System.ComponentModel.DataAnnotations;
using common.data;

namespace data.models
{
    public class Subscription : Entity
    {
        public Subscription()
        {
            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }

        public Subscription(string topic, string description, double price)
        {
            Topic = topic;
            Description = description;
            Price = price;

            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Topic { get; set; }

        [Required]
        [StringLength(10000, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int NoOfIdeas { get; set; }

        [Required]
        public int ExpiryInMonths{ get; set; }

        [Required]
        public string ColourCode { get; set; }
    }
}

