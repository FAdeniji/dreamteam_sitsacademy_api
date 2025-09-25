using System.ComponentModel.DataAnnotations;

namespace web.apis
{
	public class CampaignViewModel
	{
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public List<string>? ConversionGoal { get; set; }

        public string? UtmParameters { get; set; }

        public List<string>? TriggerEvents { get; set; }

        public List<string>? AudienceSegment { get; set; }

        public DateTime DateAdded { get; set; }

        [Required]
        public string CreatorName { get; set; }

        [Required]
        public List<PromoCodeViewModel> PromoCodes { get; set; }
    }
}

