namespace Billing.Shared.Dto;

public class OrderViewModel
{
    public int OrderId { get; set; }
    public Guid OrderNumber { get; set; }
    public Guid UserId { get; set; }
    public string Description { get; set; }
    public int ReceiptId { get; set; }
    public int PaymentId { get; set; }
}