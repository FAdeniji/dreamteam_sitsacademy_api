using System.ComponentModel.DataAnnotations;
using common.data;

namespace web.apis
{
    public class ValueFile
	{
        public IFormFile File { get; set; }
    }

    public class ValueFile1
    {
        public IFormFile File { get; set; }

        [Required]
        public DocumentType DocumentType { get; set; }

        [Required]
        public string EmailAddress { get; set; }
    }
}
