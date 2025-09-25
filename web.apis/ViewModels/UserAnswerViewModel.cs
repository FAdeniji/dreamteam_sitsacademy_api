using System.ComponentModel.DataAnnotations;

namespace web.apis
{
    public class UserAnswerViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string UserId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public int QuestionId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Text { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
