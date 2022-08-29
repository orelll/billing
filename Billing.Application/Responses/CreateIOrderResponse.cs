using Billing.Shared.Dto;

namespace Billing.Application.Responses;

public class CreateIOrderResponse: ResponseBase<ReceiptViewModelDto>
{
    public CreateIOrderResponse(ReceiptViewModelDto receiptVM):base(receiptVM)
    {
        
    }    
    
    public CreateIOrderResponse(params string[] errors):base(errors)
    {
        
    }

    public static CreateIOrderResponse Success(ReceiptViewModelDto receiptVM) => new CreateIOrderResponse(receiptVM);
    public static CreateIOrderResponse WithErrors(IEnumerable<string> errors) => new CreateIOrderResponse(errors:errors.ToArray());
    public static CreateIOrderResponse WithPaymentGatewayError() => new CreateIOrderResponse(errors: "Payment gateway returned error");
}