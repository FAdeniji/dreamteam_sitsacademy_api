using System.ComponentModel.DataAnnotations;

namespace web.apis.BindingModels
{
    public class ResetPasswordBindingModel
	{
        [Required]
		public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Code { get; set; }
    }

    public class ResetPasswordNowBindingModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class UserResetPasswordBindingModel
    {
        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}

