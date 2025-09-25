using System.ComponentModel.DataAnnotations;

namespace web.apis
{
    public class EmailTemplateViewModel
	{
        [Required]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }
    }
}

