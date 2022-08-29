
namespace Billing.Application.Entities
{
    public class Order:Entity
    {
        public int? OrderId { get; set; }

        public Guid OrderNumber { get; set; }
    
        public Guid UserId { get; set; }
    
        public string? Description { get; set; }

        public Receipt Receipt { get; set; }
    
        public int ReceiptId { get; set; }
    
        public Payment Payment { get; set; }
        public int PaymentId { get; set; }
    }
}