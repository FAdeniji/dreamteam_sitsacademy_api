using System.ComponentModel.DataAnnotations;
using common.data.Enums;

namespace web.apis
{
    public class UserRegistrationBindingModel
	{
        [DataType(DataType.Text)]
        public string Department { get; set; }

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
    }

    public class UpdateUserBingingModel {
        [Required]
        public string Id { get; set; }

        [Required]
        public UserRoleEnum UserRoleEnum { get; set; }
    }

    public class UserRegistrationUpdateBindingModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string UserId { get; set; }

        public string? Department { get; set; }
    }
}

