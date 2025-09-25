using System.ComponentModel.DataAnnotations;
using common.data.Enums;

public class ApplicationUserViewModel
{
    public string Id { get; set; }

    [Required] 
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string EmailAddress { get; set; }

    public string? UserRoleEnum { get; set; }

    public bool IsActive { get; set; }

    public string? Department { get; set; }

    public DateTime DateAdded { get; set; }
}