using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Core.Entities;

public class Order
{
    public Guid? OrderId { get; set; }

    public Guid OrderNumber { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid CurrencyId { get; set; }
    
    public int DecimalPart { get; set; }
    
    public int FullPart { get; set; }

    public Guid PaymentGateway { get; set; }
    
    public string? Description { get; set; }

    public Receipt Receipt { get; set; }
    
    [Required]
    public Guid ReceiptId { get; set; }
}