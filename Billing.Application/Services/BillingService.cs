using Billing.Application.Entities;
using Billing.Application.Models;
using Billing.Application.Responses;
using Billing.Application.Validators;
using Billing.Shared.Dto;

namespace Billing.Application.Services;

public class BillingService:IBillingService
{
    private readonly IOrdersContext _dbContext;
    private readonly IValidator<OrderModel> _orderValidator;
    private readonly IPaymentGateway _paymentGateway;

    public BillingService(IPaymentGateway paymentGateway,
        IValidator<OrderModel> orderValidator,
        IOrdersContext dbContext)
    {
        _paymentGateway = paymentGateway;
        _orderValidator = orderValidator;
        _dbContext = dbContext;
    }

    public async Task<ValidationResult> ValidateOrder(OrderModel order, CancellationToken token)
    {
        return await _orderValidator.ValidateAsync(order);
    }

    public async Task<CreateIOrderResponse> CreateOrder(OrderModel input, CancellationToken token)
    {
        //validate
        var validationResult = await _orderValidator.ValidateAsync(input);
        if (!validationResult.Ok)
        {
            return CreateIOrderResponse.WithErrors(validationResult.Errors);
        }

        //call gateway
        var paymentDetails = new PaymentModel
        {
            Currency = input.Currency,
            DecimalPart = input.DecimalPart,
            FullPart = input.FullPart,
            GatewayId = input.GatewayId
        };
        
        var gatewayResponse = await _paymentGateway.ProcessPayment(paymentDetails);

        if (!gatewayResponse.success)
        {
            return CreateIOrderResponse.WithPaymentGatewayError();
        }

        var payment = new Payment
        {
            Currency = paymentDetails.Currency,
            Timestamp = DateTime.Now,
            DecimalPart = paymentDetails.DecimalPart,
            FullPart = paymentDetails.FullPart,
            GatewayId = input.GatewayId
        };

        var order = new Order
        {
            Description = input.Description,
            UserId = input.UserId,
            OrderNumber = input.OrderNumber,
            Receipt = new Receipt(),
            Payment = payment
        };

        await _dbContext.Orders.AddAsync(order, token);
        
        await _dbContext.SaveChangesAsync(token);
 
        var receipt = _dbContext.Receipts.FirstOrDefault(rec => rec.OrderId == order.Id);


        
        return receipt != null ? CreateIOrderResponse.Success(new ReceiptViewModelDto
        {
            Description = order.Description,
            OrderId = order.Id,
            TimeStamp = payment.Timestamp.ToString(),
            UserId = order.UserId,
            PaymentGatewayId = payment.GatewayId,
            PaymentAmount = $"{payment.FullPart}.{payment.DecimalPart} {payment.Currency}"
        }) : CreateIOrderResponse.WithErrors(new []{"Receipt not found"});
    }
}