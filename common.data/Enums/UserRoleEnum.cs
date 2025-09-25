using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace common.data.Enums
{
    public enum UserRoleEnum
	{
        
        [Display(Name = "Administrator")]
        [Description("Administrator")]
        Administrator = 1,
        
        [Display(Name = "CRMManager")]
        [Description("CRMManager")]
        CRMManager = 2,
        
        [Display(Name = "CommunityManager")]
        [Description("CommunityManager")]
        CommunityManager = 3,

        [Display(Name = "CommunityMember")]
        [Description("CommunityMember")]
        CommunityMember = 4,

        [Display(Name = "CommunityEngagement")]
        [Description("CommunityEngagement")]
        CommunityEngagement = 5,

        [Display(Name = "MarketingTeamMember")]
        [Description("MarketingTeamMember")]
        MarketingTeamMember = 6
    }
}

