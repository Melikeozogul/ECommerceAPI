
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerceAPI.Infrastructure.Services;

public class CartService : ICartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }


public async Task UpdateQuantityAsync(
    string userId,
    int productId,
    int quantity)
{
    if (quantity <= 0)
        throw new InvalidOperationException(
            "Ürün adedi sıfırdan büyük olmalıdır.");

    var cartItem = await _context.CartItems
        .FirstOrDefaultAsync(c =>
            c.UserId == userId &&
            c.ProductId == productId);

    if (cartItem is null)
        throw new InvalidOperationException(
            "Ürün sepetinizde bulunamadı.");

    cartItem.Quantity = quantity;

    await _context.SaveChangesAsync();
}

public async Task ClearAsync(string userId)
{
    var cartItems = await _context.CartItems
        .Where(c => c.UserId == userId)
        .ToListAsync();

    if (cartItems.Count == 0)
        return;

    _context.CartItems.RemoveRange(cartItems);

    await _context.SaveChangesAsync();
}


    public async Task AddAsync(string userId, AddToCartDto dto)
    {
        if (dto.Quantity <= 0)
            throw new InvalidOperationException(
                "Ürün adedi sıfırdan büyük olmalıdır.");

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == dto.ProductId);

        if (product is null)
            throw new InvalidOperationException(
                "Ürün bulunamadı.");

        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c =>
                c.UserId == userId &&
                c.ProductId == dto.ProductId);

        if (cartItem is not null)
        {
            cartItem.Quantity += dto.Quantity;
        }
        else
        {
            cartItem = new CartItem
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            _context.CartItems.Add(cartItem);
        }

        await _context.SaveChangesAsync();
    }


public async Task<int> CheckoutAsync(string userId)
{
    if (string.IsNullOrWhiteSpace(userId))
        throw new InvalidOperationException(
            "Kullanıcı girişi yapılmalıdır.");

    await using var transaction =
        await _context.Database.BeginTransactionAsync();

    try
    {
        var cartItems = await _context.CartItems
            .Where(c => c.UserId == userId)
            .Include(c => c.Product)
            .ToListAsync();

        if (cartItems.Count == 0)
            throw new InvalidOperationException(
                "Sepetiniz boş.");

        foreach (var item in cartItems)
        {
            if (item.Quantity <= 0)
                throw new InvalidOperationException(
                    "Ürün adedi sıfırdan büyük olmalıdır.");

            if (item.Product.StockQuantity < item.Quantity)
                throw new InvalidOperationException(
                    $"{item.Product.Name} için yeterli stok yok.");
        }

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            TotalPrice = 0
        };

        foreach (var item in cartItems)
        {
            item.Product.StockQuantity -= item.Quantity;

            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.Product.Price
            });

            order.TotalPrice +=
                item.Product.Price * item.Quantity;
        }

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cartItems);

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


    public async Task<List<CartItemDto>> GetMyCartAsync(string userId)
    {
        return await _context.CartItems
            .Where(c => c.UserId == userId)
            .Include(c => c.Product)
            .OrderBy(c => c.Id)
            .Select(c => new CartItemDto
            {
                ProductId = c.ProductId,
                ProductName = c.Product.Name,
                UnitPrice = c.Product.Price,
                Quantity = c.Quantity
            })
            .ToListAsync();
    }

    public async Task RemoveAsync(string userId, int productId)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c =>
                c.UserId == userId &&
                c.ProductId == productId);

        if (cartItem is null)
            throw new InvalidOperationException(
                "Ürün sepetinizde bulunamadı.");

        _context.CartItems.Remove(cartItem);

        await _context.SaveChangesAsync();
    }
}
