
using ECommerceAPI.Application.DTOs;

namespace ECommerceAPI.Application.Interfaces;

public interface ICartService
{
    Task AddAsync(string userId, AddToCartDto dto);

    Task<List<CartItemDto>> GetMyCartAsync(string userId);

    Task RemoveAsync(string userId, int productId);

    Task<int> CheckoutAsync(string userId);

    Task UpdateQuantityAsync(
    string userId,
    int productId,
    int quantity);

    Task ClearAsync(string userId);
}

