using System.ComponentModel.DataAnnotations;

namespace common.data
{
    public enum EmailType
    {
        [Display(Name = "Welcome")]
        WelcomeEmail = 0,
        [Display(Name = "PaymentSuccess")]
        PaymentSuccess = 1,
        [Display(Name = "PaymentFailed")]
        PaymentFailed = 2
    }
}

