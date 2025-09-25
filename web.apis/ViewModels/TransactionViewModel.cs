using System.ComponentModel.DataAnnotations;
using common.data;
using web.apis;

public class TransactionViewModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string TransationRef { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public string Naration { get; set; }

    [Required]
    public string CustomerName { get; set; }

    [Required]
    public string CustomerEmail { get; set; }

    public int? SubscriptionId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public ProductType ProductType { get; set; }

    [Required]
    public string PaymentRequestAsJson { get; set; }

    [Required]
    public string PaymentResponseAsJson { get; set; }

    public virtual SubscriptionViewModel? Subscription { get; set; }

    public virtual ProductViewModel? Product { get; set; }

    public DateTime DateAdded { get; set; }

    public bool IsActive { get; set; }
}