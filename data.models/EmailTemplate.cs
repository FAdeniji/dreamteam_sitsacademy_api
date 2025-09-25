using common.data;
using System.ComponentModel.DataAnnotations;

namespace data.models;
public class EmailTemplate : Entity
{
    public EmailTemplate()
    {
        IsActive = true;
        DateAdded = DateTime.UtcNow;
    }

    public EmailTemplate(string code, string subject, string message)
    {
        Code = code;
        Subject = subject;
        Message = message;

        IsActive = true;
        DateAdded = DateTime.UtcNow;
    }

    [Required]
    [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public string Code { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public string Subject { get; set; }

    [Required]
    [StringLength(10000, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public string Message { get; set; }
}