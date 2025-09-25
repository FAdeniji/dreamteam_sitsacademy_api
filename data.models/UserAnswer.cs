using common.data;
using System.ComponentModel.DataAnnotations;

namespace data.models
{
    public class UserAnswer : Entity
    {
        public UserAnswer()
        {
            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }

        public UserAnswer(string userid, int questionid, string text)
        {
            UserId = userid;
            QuestionId = questionid;
            Text = text;

            IsActive = true;
            DateAdded = DateTime.UtcNow;
        }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string UserId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public int QuestionId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Text { get; set; }
    }
}
