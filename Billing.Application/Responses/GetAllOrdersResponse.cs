using Billing.Shared.Dto;

namespace Billing.Application.Responses;

public class GetAllOrdersResponse: ResponseBase<OrderViewModel[]>
{
    public GetAllOrdersResponse(OrderViewModel[] vms):base(vms)
    {
        
    }    
    
    public GetAllOrdersResponse(params string[] errors):base(errors)
    {
        
    }

    public static GetAllOrdersResponse Success(OrderViewModel[] vms) => new GetAllOrdersResponse(vms);
    public static GetAllOrdersResponse WithErrors(IEnumerable<string> errors) => new GetAllOrdersResponse(errors:errors.ToArray());
}