using Billing.Application.Models;

namespace Billing.Application.Services;

public class PaymentGateway:IPaymentGateway
{
    public async Task<(bool success, Guid paymentId)> ProcessPayment(PaymentModel payment)
    {
        return await Task.FromResult(payment.FullPart > 100 ? (false, Guid.Empty) : (true, Guid.NewGuid()));
    }
}