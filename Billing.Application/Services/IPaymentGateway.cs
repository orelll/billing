using Billing.Application.Models;

namespace Billing.Application.Services;

public interface IPaymentGateway
{
    Task<(bool success, Guid paymentId)> ProcessPayment(PaymentModel payment, CancellationToken token);
}