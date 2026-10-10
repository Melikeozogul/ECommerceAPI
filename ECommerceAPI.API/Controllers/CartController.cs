
using System.Security.Claims;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private string? GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddToCart(AddToCartDto dto)
    {
        var userId = GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        await _cartService.AddAsync(userId, dto);

        return Ok(new
        {
            message = "Ürün sepete eklendi."
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetMyCart()
    {
        var userId = GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var cart = await _cartService.GetMyCartAsync(userId);

        return Ok(new
        {
            items = cart,
            totalPrice = cart.Sum(item => item.TotalPrice)
        });
    }


[HttpPost("checkout")]
public async Task<IActionResult> Checkout()
{
    var userId = GetUserId();

    if (string.IsNullOrEmpty(userId))
        return Unauthorized();

    var orderId = await _cartService.CheckoutAsync(userId);

    return Ok(new
    {
        message = "Sipariş başarıyla oluşturuldu.",
        orderId
    });
}

    
[HttpPut("items/{productId:int}")]
public async Task<IActionResult> UpdateQuantity(
    int productId,
    [FromBody] UpdateCartQuantityDto dto)
{
    var userId = GetUserId();

    if (string.IsNullOrEmpty(userId))
        return Unauthorized();

    await _cartService.UpdateQuantityAsync(
        userId,
        productId,
        dto.Quantity);

    return Ok(new
    {
        message = "Sepet ürün miktarı güncellendi."
    });
}
    [HttpDelete]
public async Task<IActionResult> ClearCart()
{
    var userId = GetUserId();

    if (string.IsNullOrEmpty(userId))
        return Unauthorized();

    await _cartService.ClearAsync(userId);

    return Ok(new
    {
        message = "Sepetiniz temizlendi."
    });
}

    [HttpDelete("items/{productId:int}")]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        var userId = GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        await _cartService.RemoveAsync(userId, productId);

        return Ok(new
        {
            message = "Ürün sepetten kaldırıldı."
        });
    }
}
