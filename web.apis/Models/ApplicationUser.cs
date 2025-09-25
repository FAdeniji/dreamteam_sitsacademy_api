using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using common.data.Enums;
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
            DateAdded = DateTime.UtcNow;
        }

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

        public UserRoleEnum? UserRoleEnum { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateAdded { get; set; }

        [NotMapped]
        public string? Department { get; set; }

    }
}

