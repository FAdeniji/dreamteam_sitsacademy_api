using System.ComponentModel.DataAnnotations;

namespace web.apis
{
    public class ModuleViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string SoftwareActivationKey { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string ShortName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string FullName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string ProductCode { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Link { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
