
using ECommerceAPI.Application.DTOs;

namespace ECommerceAPI.Application.Interfaces;

public interface IOrderService
{
    Task<int> CreateAsync(CreateOrderDto dto, string userId);

    Task<List<OrderDto>> GetMyOrdersAsync(string userId);
}
