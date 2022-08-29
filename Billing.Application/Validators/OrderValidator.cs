using Billing.Application.Models;

namespace Billing.Application.Validators;

public class OrderValidator:IValidator<OrderModel>
{
    private readonly List<string> _allowedCurrencies = new List<string> {"eur", "pln", "usd"};

    public async Task<ValidationResult> ValidateAsync(OrderModel input)
    {
        var validationErrors = new List<string>();

        if (input.OrderNumber == Guid.Empty)
        {
            validationErrors.Add("OrderNumber contains empty data");
        }        
        if (input.UserId == Guid.Empty)
        {
            validationErrors.Add("Missing User Id");
        }        
        if (string.IsNullOrEmpty(input.Currency))
        {
            validationErrors.Add("Missing currency");
        }            
        if (!AllowedCurrency(input.Currency))
        {
            validationErrors.Add("Not supported currency");
        }        
        if (input.DecimalPart < 0 || input.FullPart < 0)
        {
            validationErrors.Add("Payment data malfunctioned");
        }        
        if (input.GatewayId == Guid.Empty)
        {
            validationErrors.Add("Missing payment service data");
        }

        return await Task.FromResult(validationErrors.Count == 0
            ? ValidationResult.WithSuccess()
            : ValidationResult.WithErrors(validationErrors.ToArray()));
    }

    private bool AllowedCurrency(string currency) => _allowedCurrencies.Any(c => c == currency.ToLower().Trim());
}