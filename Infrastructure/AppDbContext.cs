using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public required DbSet<Payment> Payments { get; init; }

    public required DbSet<Product> Products { get; init; }

    public required DbSet<Order> Orders { get; init; }

    public required DbSet<OrderProduct> OrderProducts { get; init; }

    public required DbSet<Customer> Customers { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasKey(x => x.Id);

        var payment = modelBuilder.Entity<Payment>();
        payment.HasKey(x => x.Id);
        payment.HasOne<Order>().WithMany().HasForeignKey(x => x.OrderId);

        var order = modelBuilder.Entity<Order>();
        order.HasKey(x => x.Id);
        order.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired(false);

        var orderProduct = modelBuilder.Entity<OrderProduct>();
        orderProduct.HasOne<Order>().WithMany().HasForeignKey(x => x.OrderId);
        orderProduct.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);
        orderProduct.HasKey(x => new { x.OrderId, x.ProductId });

        var customer = modelBuilder.Entity<Customer>();
        customer.HasKey(x => x.Id);
    }
}