using ECommerceAPI.Application.DTOs;

namespace ECommerceAPI.Application.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();

    Task<CategoryDto?> GetByIdAsync(int id);

    Task<CategoryDto> CreateAsync(CategoryDto categoryDto);
}