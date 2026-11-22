using Microsoft.EntityFrameworkCore;

public class OrdersAPIContext : DbContext
{
    public OrdersAPIContext(DbContextOptions<OrdersAPIContext> options) : base(options)
    {
    }

    public DbSet<OrdersAPI.Orders> Orders { get; set; } = default!;
    public DbSet<OrdersAPI.Product> Products { get; set; } = default!;
}
