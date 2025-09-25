using System;
using System.ComponentModel.DataAnnotations;

namespace web.apis.BindingModels
{
	public class ResetPasswordByEmail
	{
		[Required]
		public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

	public class ResetPasswordByEmailModel
	{
		[Required]
		public string Email { get; set; }

        [Required]
        public string Password { get; set; }

		[Required]
        public string Token { get; set; }

		[Required]
		[Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}

