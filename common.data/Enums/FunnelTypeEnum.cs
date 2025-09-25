using System.ComponentModel.DataAnnotations;

namespace common.data.Enums
{
    public enum FunnelTypeEnum
	{
        [Display(Name = "Regular")]
        Regular = 0,
        [Display(Name = "Business")]
        Business = 1,
        [Display(Name = "LearningInstitution")]
        LearningInstitution = 4
    }
}