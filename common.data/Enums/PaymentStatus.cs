using System.ComponentModel.DataAnnotations;

namespace common.data
{
    public enum PaymentStatus
    {
        [Display(Name = "PENDING")]
        PENDING = 0,
        [Display(Name = "COMPLETED")]
        COMPLETED = 1,
        [Display(Name = "FAILED")]
        FAILED = 2
    }
}

