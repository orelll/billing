using System.ComponentModel.DataAnnotations;

namespace Billing.Shared.Dto;

public class OrderDto
{
    [Required]
    public Guid OrderNumber { get; set; }
    
    [Required]
    public Guid UserId { get; set; }

    [Required] public MoneyDto PayableAmount { get; set; } = new MoneyDto { Currency = string.Empty, DecimalPart = 0, FullPart = 0};
    
    [Required]
    public Guid PaymentGateway { get; set; }
    
    
    public string? Description { get; set; }
}