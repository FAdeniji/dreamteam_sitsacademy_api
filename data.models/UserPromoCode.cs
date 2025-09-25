using System.ComponentModel.DataAnnotations.Schema;
using common.data;

namespace data.models
{
	public class UserPromoCode : Entity
    {
        public UserPromoCode()
        {
            IsActive = false;
            DateAdded = DateTime.UtcNow;
        }

        public string UserId { get; set; }

        public int PromoCodeId { get; set; }

        [ForeignKey("PromoCodeId")]
        public virtual PromoCode PromoCode { get; set; }
    }
}

