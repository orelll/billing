namespace Billing.Application.Responses;

public class ResponseBase<T>
{
    public T? Result { get; set; }
    public string[] Errors { get; set; }

    public ResponseBase()
    {
        Result = default(T);
    }
    
    public ResponseBase(params string[] errors)
    {
        Errors = errors;
        Result = default(T);
    }

    public ResponseBase(T? value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(ResponseBase<T>));
        }

        Result = value;
    }
    
    public bool Succeed => this.Result != null && !this.Errors.Any();
    
    public virtual bool NotFound => (this.Result == null || this.Result.Equals(default(T))) && !this.Errors.Any();
}