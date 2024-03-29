﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public required DbSet<Payment> Payments { get; init; }

    public required DbSet<Product> Products { get; init; }

    public required DbSet<Order> Orders { get; init; }

    public required DbSet<Reservation> Reservations { get; init; }

    public required DbSet<OrderProduct> OrderProducts { get; init; }

    public required DbSet<Customer> Customers { get; init; }

    public required DbSet<Employee> Employees { get; init; }

    public required DbSet<ServiceEmployee> ServiceEmployees { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var product = modelBuilder.Entity<Product>();
        product.HasKey(x => x.Id);
        product.HasIndex(x => x.Name).IsUnique();

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

        var serviceEmployee = modelBuilder.Entity<ServiceEmployee>();
        serviceEmployee.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId);
        serviceEmployee.HasOne<Service>().WithMany().HasForeignKey(x => x.ServiceId);
        serviceEmployee.HasKey(x => new { x.EmployeeId, x.ServiceId });

        var customer = modelBuilder.Entity<Customer>();
        customer.HasKey(x => x.Id);
    }
}