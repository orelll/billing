namespace Billing.Application.Models;

public class OrderModel : Model
{
    public Guid OrderNumber { get; set; }
    
    public Guid UserId { get; set; }
    
    public string? Description { get; set; }

    public Guid GatewayId { get; set; }
    public DateTime Timestamp { get; set; }
    public int FullPart { get; set; }
    public int DecimalPart { get; set; }
    public string Currency { get; set; }
}