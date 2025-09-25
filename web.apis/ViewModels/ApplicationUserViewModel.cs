using System.ComponentModel.DataAnnotations;
using common.data.Enums;

public class ApplicationUserViewModel
{
    public string Id { get; set; }

    public string OrganisationName { get; set; }

    [Required] 
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string EmailAddress { get; set; }

    [Required]
    public string MobileNumber { get; set; }

    public string? UserRoleEnum { get; set; }

    public bool IsActive { get; set; }

    public string? Website { get; set; }

    public string? Description { get; set; }

    public DateTime DateAdded { get; set; }
}

public class InvestorViewModel2
{
    public string Id { get; set; }

    public string OrganisationName { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    public UserRoleEnum? UserRoleEnum { get; set; }

    public bool IsActive { get; set; }

    public string? Website { get; set; }

    public string? Description { get; set; }
}