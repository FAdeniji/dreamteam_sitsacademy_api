using System.ComponentModel.DataAnnotations;
namespace web.apis.ViewModels
{
    public class NotificationViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string RecipientName { get; set; }

        [Required]
        [EmailAddress]
        public string RecipientEmailAddress { get; set; }

        [Required]
        [Phone]
        [MinLength(11)]
        [MaxLength(21)]
        public string RecipientPhoneNumber { get; set; }

        [Required]
        public string SenderName { get; set; }

        [Required]
        public string SenderEmailAddress { get; set; }

        public bool IsSent { get; set; }

        public bool IsLoaded { get; set; }

        public int NoOfAttempts { get; set; }

        public string? UserId { get; set; }

        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }
    }
}