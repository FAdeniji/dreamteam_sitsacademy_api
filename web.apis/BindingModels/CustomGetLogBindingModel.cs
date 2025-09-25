using System.ComponentModel.DataAnnotations;

public class CustomGetLogBindingModel
{
    [Required]
    public DateTime Start { get; set; }

    [Required]
    public DateTime End { get; set; }

    [Required]
    public string Name { get; set; }
}