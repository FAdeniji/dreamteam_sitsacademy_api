using System.ComponentModel.DataAnnotations;

namespace common.data
{
    public enum DocumentType
    {
        [Display(Name = "Others")]
        Others = 1,
        [Display(Name = "ProfilePicture")]
        ProfilePicture = 2
    }
}
