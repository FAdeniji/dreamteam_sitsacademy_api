using common.data;
using System.ComponentModel.DataAnnotations;

namespace data.models
{
    public class Course : Entity
    {
        public Course()
        {
            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }

        public Course(string shortname, string fullname, string productcode)
        {
            ShortName = shortname;
            Fullname = fullname;
            ProductCode = productcode;

            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string ShortName { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Fullname { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string ProductCode { get; set; }
    }
}
