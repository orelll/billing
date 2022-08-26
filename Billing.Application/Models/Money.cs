namespace Billing.Application.Models;

public class Money
{
    public int FullPart { get; set; }
    public int DecimalPart { get; set; }
    public string Currency { get; set; }
}