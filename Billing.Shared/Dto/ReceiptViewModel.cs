namespace Billing.Shared.Dto;

public class ReceiptViewModelDto
{
    public int OrderId { get; set; }
    public Guid UserId { get; set; }
    public string Description { get; set; }
    public Guid PaymentGatewayId { get; set; }
    public string TimeStamp { get; set; }
    public string PaymentAmount { get; set; }
}