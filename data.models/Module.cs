using common.data;
using System.ComponentModel.DataAnnotations;

namespace data.models
{
    public class Module : Entity
    {
        public Module()
        {
            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }


        public Module(string softwareactivationkey, string shortname, string fullname, string productcode, string? link)
        {
            SoftwareActivationKey = softwareactivationkey;
            ShortName = shortname;
            FullName = fullname;
            ProductCode = productcode;

            IsActive = true;
            DateAdded = DateTime.UtcNow;
            Link = link;
        }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string SoftwareActivationKey { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string ShortName { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string FullName { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string ProductCode { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string? Link { get; set; }
    }
}
