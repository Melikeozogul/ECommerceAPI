
using System.Security.Claims;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var orderId = await _orderService.CreateAsync(dto, userId);

        return Ok(new
        {
            Message = "Sipariş başarıyla oluşturuldu.",
            OrderId = orderId
        });
        
    }

[HttpGet("my-orders")]
public async Task<IActionResult> GetMyOrders()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? User.FindFirstValue("sub");

    if (string.IsNullOrEmpty(userId))
    {
        return Unauthorized();
    }

    var orders = await _orderService.GetMyOrdersAsync(userId);

    return Ok(orders);
}

}
