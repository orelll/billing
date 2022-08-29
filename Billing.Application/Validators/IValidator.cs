using Billing.Application.Models;

namespace Billing.Application.Validators;

public interface IValidator<in TIn> where TIn: Model
{
    Task<ValidationResult> ValidateAsync(TIn input);
}