using System.ComponentModel.DataAnnotations;

namespace web.apis
{
    public class CourseViewModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string SoftwareActivationKey { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Fullname { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string ProductCode { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
