namespace Billing.Shared.Dto;

public class MoneyDto
{
    public int FullPart { get; set; }
    public int DecimalPart { get; set; }
    public string Currency { get; set; }
}