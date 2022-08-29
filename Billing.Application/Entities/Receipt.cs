using System.ComponentModel.DataAnnotations;

namespace Billing.Application.Entities;

public class Receipt:Entity
{
    [Required]
    public Order Order { get; set; }
    public int OrderId { get; set; }
}