using System.ComponentModel.DataAnnotations;
using common.data;

namespace data.models
{
    public class Campaign : Entity
    {
        public Campaign()
        {
            IsActive = true;
            DateAdded = DateTime.UtcNow;
            UtmParameters += $"{DateTime.UtcNow.ToString("ddmmyyyy")}{Security.RandomString(4)},";
            PromoCodes = new HashSet<PromoCode>();
        }

        public Campaign(string name, string description, List<string>? conversionGoal, string? utmParameters, List<string>? triggerEvents, List<string>? audienceSegment, string creatorUserId)
        {
            IsActive = true;
            DateAdded = DateTime.UtcNow;
            UtmParameters += $"{DateTime.UtcNow.ToString("ddmmyyyy")}{Security.RandomString(4)},";
            PromoCodes = new HashSet<PromoCode>();
            CreatorUserId = creatorUserId;

            Name = name;
            Description = description;
            ConversionGoal = conversionGoal;
            TriggerEvents = triggerEvents;
            AudienceSegment = audienceSegment;
        }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public List<string>? ConversionGoal { get; set; }

        public string? UtmParameters { get; set; }

        public List<string>? TriggerEvents { get; set; }

        public List<string>? AudienceSegment { get; set; }

        [Required]
        public string CreatorUserId { get; set; }

        public ICollection<PromoCode> PromoCodes { get; set; }

        public void AddPromoCodes(PromoCode promoCode)
        {
            if (!PromoCodes.Contains(promoCode))
            {
                PromoCodes.Add(promoCode);
            }
        }

        public void RemovePromoCodes(PromoCode promoCode)
        {
            if (PromoCodes.Contains(promoCode))
            {
                PromoCodes.Remove(promoCode);
            }
        }
    }
}

