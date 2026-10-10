using ECommerceAPI.Application.DTOs;

namespace ECommerceAPI.Application.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();

    Task<ProductDto?> GetByIdAsync(int id);

    Task<ProductDto> CreateAsync(CreateProductDto productDto);

    Task<ProductDto?> UpdateAsync(int id, UpdateProductDto productDto);

    Task<bool> DeleteAsync(int id);
}