using System.ComponentModel.DataAnnotations;

namespace web.apis
{
    public class UserViewModel
	{
        public UserViewModel()
        {
            
        }

        [Required]
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]        
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EmailAddress { get; set; }
        
        [Required]
        public string UserRole { get; set; }

        public int CurrentNoOfIdeas { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateAdded { get; set; }

        public int? LearningInstitutionId { get; set; }
    }
}

