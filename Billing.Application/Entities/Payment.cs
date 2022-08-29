namespace Billing.Application.Entities;

public class Payment:Entity
{
    public Guid GatewayId { get; set; }
    public DateTime Timestamp { get; set; }
    public int FullPart { get; set; }
    public int DecimalPart { get; set; }
    public string Currency { get; set; }
    public Order Order { get; set; }
    public int OrderId { get; set; }
}