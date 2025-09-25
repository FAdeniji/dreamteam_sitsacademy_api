using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using common.data.Enums;
using data.models;
using Microsoft.AspNetCore.Identity;

namespace web.apis.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() {
            SecurityStamp = Guid.NewGuid().ToString();
            EmailConfirmed = true;
            PhoneNumberConfirmed = true;
            IsActive = true;
            Documents = new HashSet<Document>();
            DateAdded = DateTime.UtcNow;
        }

        [MaxLength(500)] // EF makes it 50
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string OrganisationName { get; set; }

        [Required]
        [MaxLength(500)] // EF makes it 50
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(500)] // EF makes it 50
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(500)] // EF makes it 50
        [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(21)] // EF makes it 50
        [StringLength(21, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string PhoneNumber { get; set; }

        public UserRoleEnum? UserRoleEnum { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateAdded { get; set; }

        public string? Description { get; set; }

        public ICollection<Document>? Documents { get; set; }

        [NotMapped]
        public string? Website { get; set; }

        public int? GroupID { get; set; }

        [NotMapped]
        public string? InvestorType { get; set; }

        public int? LearningInstitutionId { get; set; }

        public void AddDocument(Document document)
        {
            Documents.Add(document);
        }

        public void RemoveDocument(Document document)
        {
            Documents.Remove(document);
        }
    }
}

