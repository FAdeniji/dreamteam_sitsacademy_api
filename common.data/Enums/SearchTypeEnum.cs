using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace common.data.Enums
{
	public enum SearchTypeEnum
	{
        [Display(Name = "Search")]
        Search = 0,
        [Display(Name = "Nlp")]
        Nlp = 1
    }
}

