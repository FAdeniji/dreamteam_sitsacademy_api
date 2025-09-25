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

        public Course(string softwareactivationkey, string fullname, string productcode)
        {
            SoftwareActivationKey = softwareactivationkey;
            Fullname = fullname;
            ProductCode = productcode;

            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string SoftwareActivationKey { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Fullname { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string ProductCode { get; set; }
    }
}
