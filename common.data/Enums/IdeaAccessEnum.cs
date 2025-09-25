using System;
using System.ComponentModel.DataAnnotations;

namespace common.data.Enums
{
	public enum IdeaAccessEnum
	{
        [Display(Name = "NoAccess")]
        NoAccess = 0,
        [Display(Name = "Viewer")]
		Viewer = 1,
        [Display(Name = "Commenter")]
        Commenter = 2,
        [Display(Name = "Editor")]
        Editor = 3
	}
}

