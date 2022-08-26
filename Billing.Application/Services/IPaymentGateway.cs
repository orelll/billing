using Billing.Application.Models;

namespace Billing.Application.Services;

public interface IPaymentGateway
{
    Task<bool> ProcessPayment(Guid gatewayId, Money amount);
}