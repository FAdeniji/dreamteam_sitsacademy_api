using common.data;
using System.ComponentModel.DataAnnotations;

namespace data.models
{
    public class Question : Entity
    {
        public Question()
        {
            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }

        public Question(int moduleid, string text)
        {
            ModuleId = moduleid;
            Text = text;

            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public int ModuleId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Text { get; set; }
    }
}
