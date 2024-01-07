using Domain.Models;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using WebApi.Interfaces;
using WebApi.Services;

namespace WebApi.Extensions;

public static class ServicesExtensions
{
    public static void AddDatabaseServices(this IServiceCollection services)
    {
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IOrderProductRepository, OrderProductRepository>();
        services.AddScoped<IGenericRepository<Customer>, CustomerRepository>();
        services.AddScoped<IGenericRepository<Employee>, EmployeeRepository>();
        services.AddScoped<IGenericRepository<Product>, ProductRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IGenericRepository<Customer>, CustomerRepository>();
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IOrderProductService, OrderProductService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<ICustomerService, CustomerService>();
    }
}
