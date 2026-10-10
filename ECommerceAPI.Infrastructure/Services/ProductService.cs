using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        return await _context.Products
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId
            })
            .ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            StockQuantity = productDto.StockQuantity,
            ImageUrl = productDto.ImageUrl,
            CategoryId = productDto.CategoryId
        };

        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId
        };
    }

        public async Task<ProductDto?> UpdateAsync(
    int id,
    UpdateProductDto productDto)
{
    var product = await _context.Products.FindAsync(id);

    if (product is null)
        return null;

    product.Name = productDto.Name;
    product.Description = productDto.Description;
    product.Price = productDto.Price;
    product.StockQuantity = productDto.StockQuantity;
    product.ImageUrl = productDto.ImageUrl;
    product.CategoryId = productDto.CategoryId;

    await _context.SaveChangesAsync();

    return new ProductDto
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        StockQuantity = product.StockQuantity,
        ImageUrl = product.ImageUrl,
        CategoryId = product.CategoryId
    };
}

    public async Task<bool> DeleteAsync(int id)
{
    var product = await _context.Products.FindAsync(id);

    if (product is null)
        return false;

    _context.Products.Remove(product);

    await _context.SaveChangesAsync();

    return true;
}
}
    
