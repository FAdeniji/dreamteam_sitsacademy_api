using System.ComponentModel.DataAnnotations;

namespace web.apis
{
    public class DocumentBindingModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string TypeId { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string ActualFileName { get; set; }

        [Required]
        public string Path { get; set; }

        public string? Url { get; set; }

        public string? Description { get; set; }
    }

    public class DocumentUpdateBindingModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string TypeId { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string ActualFileName { get; set; }

        [Required]
        public string Path { get; set; }
    }
}