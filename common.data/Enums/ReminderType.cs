using System.ComponentModel.DataAnnotations;

namespace common.data.Enums
{
    public enum ReminderType
	{
        [Display(Name = "Both")]
        Both = 0,
        [Display(Name = "Email")]
        Email = 1,
        [Display(Name = "SMS")]
        SMS = 2
    }
}

