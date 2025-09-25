using System.ComponentModel.DataAnnotations;

namespace web.apis
{
    public class SubscriptionBindingModel
	{       
        [Required]
        public string Topic { get; set; }

        [Required]
        public string Description { get; set; }

        public string? ColourCode { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int NoOfIdeas { get; set; }

        [Required]
        public string ExpiryInMonths { get; set; }
    }

    public class SubscriptionUpdateBindingModel
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
        public int ExpiryInMonths { get; set; }

        [Required]
        public int NoOfIdeas { get; set; }

        public string? ColourCode { get; set; }
    }
}

