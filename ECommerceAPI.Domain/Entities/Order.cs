namespace ECommerceAPI.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }

    public DateTime OrderDate { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}