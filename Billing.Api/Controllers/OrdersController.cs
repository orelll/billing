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
    
    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> GetList()
    {
        return Ok();
    }
    
    [HttpGet]
    [Route("{id}/status")]
    [Produces("application/json")]
    public async Task<IActionResult> GetStatus(Guid orderId)
    {
        return Ok();
    }
    
    [HttpPost]
    [Produces("application/json")]
    public async Task<IActionResult> NewOrder([FromBody] OrderDto order, CancellationToken token)
    {

        var creationResponse = await _billingService.CreateOrder(order.ToOrderModel(), token);
        
        return Ok(creationResponse);
    }    
    
    [HttpDelete]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteOrder(Guid orderId)
    {
        return Ok();
    }
}