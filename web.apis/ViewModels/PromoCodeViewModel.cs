using System.ComponentModel.DataAnnotations;

namespace web.apis
{
	public class PromoCodeViewModel
	{
        [Required]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        public string DisplayName { get; set; }

        public string CreatorName { get; set; }

        [Required]
        public int Value { get; set; }

        public string? Applicability { get; set; }

        public int MinCartValue { get; set; }

        public bool EligibleForAllUsers { get; set; }

        public bool EligibleForNewUsers { get; set; }

        public int UsageLimit { get; set; }

        public DateTime DateAdded { get; set; }
    }
}

