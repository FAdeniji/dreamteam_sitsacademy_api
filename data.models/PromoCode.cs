using System.ComponentModel.DataAnnotations;
using common.data;

namespace data.models
{
	public class PromoCode : Entity
    {
        public PromoCode()
        {
            IsActive = true;
            DateAdded = DateTime.UtcNow;
            Code = $"{DateTime.UtcNow.ToString("ddmmyyyy")}-{Security.RandomString(8)}";
        }

        [Required]
        public string Code { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PromoCode other && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public int Value { get; set; }

        public string? Applicability { get; set; }

        public int MinCartValue { get; set; }

        public bool EligibleForAllUsers { get; set; }

        public bool EligibleForNewUsers { get; set; }

        public int UsageLimit { get; set; }

        public string CreatorUserId { get; set; }

        public virtual UserPromoCode? UserPromoCode { get; set; }

        public void UsedPromoCode(UserPromoCode usedPromoCode)
        {
            UserPromoCode = usedPromoCode;
        }
    }
}

