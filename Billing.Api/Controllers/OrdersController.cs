using Billing.Api.ExtensionMethods;
using Billing.Application.Services;
using Billing.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Api.Controllers;

[ApiController]
[Route(Route)]
public class OrdersController : ControllerBase
{
    private const string Route = "api/orders";
    private readonly IBillingService _billingService;
    
    public OrdersController(IBillingService billingService)
    {
        _billingService = billingService;
    }
    
    /// <summary>
    /// Endpoint dedicated for listing all persisted orders
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderViewModel[]))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
    public async Task<IActionResult> GetList(CancellationToken token)
    {
        var getAllResponse = await _billingService.GetAll(token);

        return getAllResponse.Succeed ? new ObjectResult(getAllResponse.Result) : BadRequest(getAllResponse.Errors);
    }
    
    [HttpGet]
    [Route("{id}/status")]
    [Produces("application/json")]
    public async Task<IActionResult> GetStatus(Guid orderId)
    {
        return Ok();
    }
    
    /// <summary>
    /// Endpoint dedicated for order creation
    /// </summary>
    /// <param name="order"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReceiptViewModelDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
    public async Task<IActionResult> NewOrder([FromBody] OrderDto order, CancellationToken token)
    {

        var creationResponse = await _billingService.CreateOrder(order.ToOrderModel(), token);

        return creationResponse.Succeed ? new OkObjectResult(creationResponse.Result) : BadRequest(creationResponse.Errors);
    }    
    
    [HttpDelete]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteOrder(Guid orderId)
    {
        return Ok();
    }
}