using System.ComponentModel.DataAnnotations;

namespace common.data
{
    public enum EntityType
	{
        [Display(Name = "Note")]
        Note = 0,
        [Display(Name = "Idea")]
        Idea = 1,
        [Display(Name = "Account")]
        Account = 2
    }
}

