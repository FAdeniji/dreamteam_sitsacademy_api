using System.ComponentModel.DataAnnotations;

public class CustomStringBindingModel
{
	[Required]
	public string Email { get; set; }
}

public class CustomTextBindingModel
{
    [Required]
    public string Text { get; set; }
}