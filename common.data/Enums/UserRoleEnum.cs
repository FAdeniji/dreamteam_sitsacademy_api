using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace common.data.Enums
{
    public enum UserRoleEnum
	{
        
        [Display(Name = "Administrator")]
        [Description("Administrator")]
        Administrator = 1,
        
        [Display(Name = "Staff")]
        [Description("Staff")]
        Staff = 2,

        [Display(Name = "User")]
        [Description("User")]
        User = 3
    }
}

