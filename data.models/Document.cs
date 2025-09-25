using System.ComponentModel.DataAnnotations;
using common.data;

namespace data.models
{
    public class Document : Entity
    {
        public Document(DocumentType typeId, string filename, string actualFileName, string path,
            string userId, int accessDuration, int assetId, string url = "", string description = "")
        {
            TypeId = typeId;
            FileName = filename;
            ActualFileName = actualFileName;
            Path = path;
            IsActive = true;
            DateAdded = DateTime.UtcNow;
            UserId = userId;
            ExpiryDate = DateTime.UtcNow.AddHours(accessDuration);
            AssetId = assetId;
            Url = url;
            Description = description;
        }

        public Document()
        {
            IsActive = true;
            DateAdded = DateTime.UtcNow;
            ExpiryDate = DateTime.UtcNow.AddHours(2); // default of 2 hours
        }

        [Required]
        public string UserId { get; set; }

        public int AssetId { get; set; }

        [Required]
        public DocumentType TypeId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string FileName { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string ActualFileName { get; set; }

        [StringLength(5000, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string? Url { get; set; }

        [StringLength(5000, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string? Description { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Path { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}


