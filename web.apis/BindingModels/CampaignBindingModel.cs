using System.ComponentModel.DataAnnotations;

namespace web.apis
{
	public class CampaignBindingModel
	{
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public List<string>? ConversionGoal { get; set; }

        public string? UtmParameters { get; set; }

        public List<string>? TriggerEvents { get; set; }

        public List<string>? AudienceSegment { get; set; }

        [Required]
        public List<int> PromoCodeIds { get; set; }
    }

    public class CampaignUpdateBindingModel
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

        [Required]
        public List<int> PromoCodeIds { get; set; }
    }
}

