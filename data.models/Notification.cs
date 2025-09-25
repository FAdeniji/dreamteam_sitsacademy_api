using System.ComponentModel.DataAnnotations;
using common.data;

namespace data.models
{
    public class Notification : Entity
    {
        public Notification(string recipientname, string recipientemail, string recipientphone, string sendername, string senderemail, string subject, string message, string userId)
        {
            RecipientName = recipientname;
            RecipientEmailAddress = recipientemail;
            RecipientPhoneNumber = recipientphone;
            SenderName = sendername;
            SenderEmailAddress = senderemail;
            IsActive = true;
            DateAdded = DateTime.UtcNow;
            Subject = subject;
            Message = message;
            UserId = userId;

            AddedBy = "Hydreate";
            UpdatedBy = "Hydreate";
        }

        public Notification()
        {
            IsActive = true;
            DateAdded = DateTime.UtcNow;
            AddedBy = "Hydreate";
            UpdatedBy = "Hydreate";
        }

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

        public string UserId { get; set; }

        #region IEntity
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        #endregion
    }
}

