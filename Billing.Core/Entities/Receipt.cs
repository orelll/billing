using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Core.Entities;

public class Receipt
{
    [Required]
    public Order Order { get; set; }
    public Guid OrderId { get; set; }
    public Guid Id { get; set; }
    public int FullPart { get; set; }
    public int DecimalPart { get; set; }
    public string Currency { get; set; }
}