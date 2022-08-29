using Billing.Application.Models;
using Billing.Shared.Dto;

namespace Billing.Api.ExtensionMethods;

public static class DtoExtensionMethods
{
    public static OrderModel ToOrderModel(this OrderDto dto) => new OrderModel
    {
        Currency = dto.PayableAmount.Currency,
        FullPart = dto.PayableAmount.FullPart,
        DecimalPart = dto.PayableAmount.DecimalPart,
        Description = dto.Description,
        GatewayId = dto.PaymentGateway,
        OrderNumber = dto.OrderNumber,
        UserId = dto.UserId
    };
}