using System.ComponentModel.DataAnnotations;

namespace web.apis
{
    public class DocumentViewModel
	{
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string TypeId { get; set; }

        [Required]
        public string FileName { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        [Required]
        public string ActualFileName { get; set; }

        [Required]
        public string Path { get; set; }

        public DateTime ExpiryDate { get; set; }

        public UserViewModel UserViewModel { get; set; }

        public DateTime DateAdded { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}

