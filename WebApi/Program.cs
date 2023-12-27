using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Domain.Models;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using WebApi.Endpoints;
using WebApi.Interfaces;
using WebApi.Services;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IOrderProductRepository, OrderProductRepository>();
builder.Services.AddScoped<IGenericRepository<Customer>, CustomerRepository>();
builder.Services.AddScoped<IGenericRepository<Employee>, EmployeeRepository>();
builder.Services.AddScoped<IGenericRepository<Order>, OrderRepository>();
builder.Services.AddScoped<IGenericRepository<Service>, ServiceRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "PSPPoS API",
        Description = "An ASP.NET Core Web API for a PoS system.",
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.MapPaymentEndpoints();
// Will need fixing
//app.MapOrderEndpoints();
//app.MapProductEndpoints();
//app.MapCustomerEndpoints();

using var scope = app.Services.CreateScope();
using var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
ctx.Database.Migrate();

app.Run();

