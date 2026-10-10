
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }


public async Task<int> CreateAsync(CreateOrderDto dto, string userId)
{
    if (string.IsNullOrWhiteSpace(userId))
        throw new InvalidOperationException("User must be logged in.");

    if (dto.Items == null || dto.Items.Count == 0)
        throw new InvalidOperationException("An order must contain at least one item.");

    if (dto.Items.Any(item => item.Quantity <= 0))
        throw new InvalidOperationException("Quantity must be greater than zero.");

    // Aynı ürün birden fazla satırda gönderilmesin.
    if (dto.Items.GroupBy(item => item.ProductId).Any(group => group.Count() > 1))
        throw new InvalidOperationException("A product can only appear once in an order.");

    await using var transaction =
        await _context.Database.BeginTransactionAsync();

    try
    {
        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            TotalPrice = 0
        };

        foreach (var item in dto.Items)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == item.ProductId);

            if (product is null)
                throw new InvalidOperationException(
                    $"Product not found: {item.ProductId}");

            if (product.StockQuantity < item.Quantity)
                throw new InvalidOperationException(
                    $"Insufficient stock for {product.Name}.");

            product.StockQuantity -= item.Quantity;

            order.OrderItems.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });

            order.TotalPrice += product.Price * item.Quantity;
        }

        _context.Orders.Add(order);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return order.Id;
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}


public async Task<List<OrderDto>> GetMyOrdersAsync(string userId)
{
    return await _context.Orders
        .Where(o => o.UserId == userId)
        .Include(o => o.OrderItems)
            .ThenInclude(item => item.Product)
        .OrderByDescending(o => o.OrderDate)
        .Select(o => new OrderDto
        {
            Id = o.Id,
            TotalPrice = o.TotalPrice,
            OrderDate = o.OrderDate,
            Items = o.OrderItems.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        })
        .ToListAsync();
}

}
