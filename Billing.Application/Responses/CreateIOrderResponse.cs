namespace Billing.Application.Responses;

public class CreateIOrderResponse: ResponseBase<Guid>
{
    public CreateIOrderResponse(Guid orderId):base(orderId)
    {
        
    }    
    
    public CreateIOrderResponse(params string[] errors):base(errors)
    {
        
    }

    public static CreateIOrderResponse Success(Guid orderId) => new CreateIOrderResponse(orderId);
}