using Billing.Application.Responses;
using Billing.Core.Entities;

namespace Billing.Application.Services;

public interface IBillingService
{
    Task<bool> ValidateOrder(Order order);
    Task<CreateIOrderResponse> CreateOrder(Order order);
} 