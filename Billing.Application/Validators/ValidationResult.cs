namespace Billing.Application.Validators;

public class ValidationResult
{
    public bool Ok { get; init; }
    public string[] Errors { get; init; }

    public static ValidationResult WithSuccess() => new ()
    {
        Ok = true,
        Errors = Array.Empty<string>()
    };
    public static ValidationResult WithErrors(params string[] errors) => new ()
    {
        Errors = errors,
        Ok = false
    };
}