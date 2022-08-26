using Billing.Application.Responses;
using Billing.Core.Entities;

namespace Billing.Application.Services;

public class BillingService:IBillingService
{
    private readonly IPaymentGateway _paymentGateway;

    public BillingService(IPaymentGateway paymentGateway)
    {
        this._paymentGateway = paymentGateway;
    }
    
    public Task<bool> ValidateOrder(Order order)
    {
        throw new NotImplementedException();
    }

    public Task<CreateIOrderResponse> CreateOrder(Order order)
    {
        throw new NotImplementedException();
    }
}