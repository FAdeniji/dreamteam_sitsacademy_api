using System;
using System.ComponentModel.DataAnnotations;

namespace web.apis.BindingModels
{
	public class UserLoginBindingModel
	{
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

