using System.ComponentModel.DataAnnotations;
using common.data;

namespace data.models
{
    public class EntityPulse : Entity
    {
        public EntityPulse()
        {
            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }

        [Required]
        [MaxLength(200)] // EF makes it 50
        [StringLength(200, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Entity { get; set; }

        [Required]
        public int EntityId { get; set; }

        [Required]
        public int SubEntityId { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}

