using System.ComponentModel.DataAnnotations;

namespace web.apis.BindingModels
{
	public class ForgotPasswordViewModel
	{
        [Required]
        public string Email { get; set; }
    }
}

