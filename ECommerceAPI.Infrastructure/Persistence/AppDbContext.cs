using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{

protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    builder.Entity<ECommerceAPI.Domain.Entities.Order>()
        .HasOne<ECommerceAPI.Infrastructure.Identity.ApplicationUser>()
        .WithMany()
        .HasForeignKey(order => order.UserId)
        .OnDelete(DeleteBehavior.Restrict);
}

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
}