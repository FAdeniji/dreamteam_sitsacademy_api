using System.ComponentModel.DataAnnotations;
using common.data.Enums;

namespace web.apis
{
    public class UserRegistrationBindingModel
	{
        [DataType(DataType.Text)]
        public string OrganisationName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string? UserRoleEnum { get; set; }

        public string? InvestorType { get; set; }

        public string? WebsiteUrl { get; set; }

        [Required]
        public int SubscriptionId { get; set; }

        public string? Description { get; set; }

        public string? Address { get; set; }

        public int? LearningInstitutionId { get; set; }

        public int? CourseId { get; set; }
    }

    public class UpdateUserBingingModel {
        [Required]
        public string Id { get; set; }

        [Required]
        public UserRoleEnum UserRoleEnum { get; set; }
    }

    public class UserRegistrationUpdateBindingModel
    {
        public string? OrganisationName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string MobileNumber { get; set; }

        public bool IsInvestor { get; set; }

        [Required]
        public string UserId { get; set; }

        public string? InvestorType { get; set; }

        public string? WebsiteUrl { get; set; }

        [Required]
        public int SubscriptionId { get; set; }

        public string? Description { get; set; }
    }
}

