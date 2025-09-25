using System.ComponentModel.DataAnnotations;

namespace common.data
{
    public enum CurrencyType
	{
        [Display(Name = "gbp", ShortName = "gbp")]
        Pounds = 0,
        [Display(Name = "usd", ShortName = "usd")]
        Dollars = 1,
        [Display(Name = "ngn", ShortName = "ngn")]
        Naira = 2
    }
}

