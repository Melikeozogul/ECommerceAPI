using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;


namespace ECommerceAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICartService, CartService>();

        return services;
    }
}