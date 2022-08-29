namespace Billing.Application.Models;

public class PaymentModel : Model
{
    public Guid GatewayId { get; set; }
    public int FullPart { get; set; }
    public int DecimalPart { get; set; }
    public string Currency { get; set; }
}