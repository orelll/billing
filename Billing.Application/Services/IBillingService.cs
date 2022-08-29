using Billing.Application.Models;
using Billing.Application.Responses;
using Billing.Application.Validators;

namespace Billing.Application.Services;

public interface IBillingService
{
    Task<ValidationResult> ValidateOrder(OrderModel order, CancellationToken token);
    Task<CreateIOrderResponse> CreateOrder(OrderModel input, CancellationToken token);
    Task<GetAllOrdersResponse> GetAll(CancellationToken token);
} 