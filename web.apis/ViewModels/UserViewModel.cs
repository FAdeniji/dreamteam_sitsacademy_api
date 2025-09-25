using System.ComponentModel.DataAnnotations;

namespace web.apis
{
    public class UserViewModel
	{
        public UserViewModel()
        {
            IsInvestor = false;
        }

        [Required]
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string OrganisationName { get; set; }

        [Required]        
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserRole { get; set; }

        public int CurrentNoOfIdeas { get; set; }

        public int AllowedNumberOfFreeIdeas { get; set; }

        public bool IsInvestor { get; set; }

        public string? Website { get; set; }

        public string InvestorType { get; set; }

        public bool IsActive { get; set; }

        public string? Description { get; set; }

        public string? Subscription { get; set; }

        public DateTime DateAdded { get; set; }

        public int? LearningInstitutionId { get; set; }

        public List<DocumentViewModel> Documents { get; set; }
    }
}

