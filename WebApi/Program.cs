using Infrastructure;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateSlimBuilder(args);

//builder.Services.ConfigureHttpJsonOptions(options =>
//{

//});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

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

var customerApi = app.MapGroup("/customer").WithTags("Customers").WithOpenApi();

customerApi.MapGet("/{id}", (Guid id) =>
{
    throw new NotImplementedException();
});

customerApi.MapPut("/{id}", (Guid id) =>
{
    throw new NotImplementedException();
});

customerApi.MapPost("/", () =>
{
    throw new NotImplementedException();
});

customerApi.MapDelete("/{id}", (Guid id) =>
{
    throw new NotImplementedException();
});

var ordersApi = app.MapGroup("/order").WithTags("Orders").WithOpenApi();

ordersApi.MapGet("/", () =>
{
    throw new NotImplementedException();
});

ordersApi.MapPost("/", () =>
{
    throw new NotImplementedException();
});

ordersApi.MapPut("/{id}", (Guid id) =>
{
    throw new NotImplementedException();
});

ordersApi.MapGet("/{id}", (Guid id) =>
{
    throw new NotImplementedException();
});

ordersApi.MapDelete("/{id}", (Guid id) =>
{
    throw new NotImplementedException();
});

var paymentApi = app.MapGroup("/payment").WithTags("Payments");

paymentApi.MapPost("/", () =>
{
    throw new NotImplementedException();
});

paymentApi.MapGet("/validate", () =>
{
    throw new NotImplementedException();
});

var reservationApi = app.MapGroup("/reservation").WithTags("Reservations");

reservationApi.MapGet("/", () =>
{
    throw new NotImplementedException();
});

reservationApi.MapPost("/", () =>
{
    throw new NotImplementedException();
});

reservationApi.MapPut("/{id}", (Guid id) =>
{
    throw new NotImplementedException();
});

reservationApi.MapGet("/{id}", (Guid id) =>
{
    throw new NotImplementedException();
});

reservationApi.MapDelete("/{id}", (Guid id) =>
{
    throw new NotImplementedException();
});

var inventoryApi = app.MapGroup("/inventory").WithTags("Inventory");

inventoryApi.MapGet("/{itemId}/availability", (Guid itemId) =>
{
    throw new NotImplementedException();
});

inventoryApi.MapPut("/{itemId}", (Guid itemId) =>
{
    throw new NotImplementedException();
});

var employeeApi = app.MapGroup("/employee").WithTags("Employees");

employeeApi.MapGet("/", () =>
{
    throw new NotImplementedException();
});

employeeApi.MapPost("/{id}/loghours", (Guid id) =>
{
    throw new NotImplementedException();
});

employeeApi.MapGet("/{id}/schedule", (Guid id) =>
{
    throw new NotImplementedException();
});

employeeApi.MapPut("/{id}/schedule", (Guid id) =>
{
    throw new NotImplementedException();
});

var loyaltyApi = app.MapGroup("/loyalty").WithTags("Loyalty program");

loyaltyApi.MapPost("/enroll", () =>
{
    throw new NotImplementedException();
});

loyaltyApi.MapPost("/{id}/applyBenefits", (Guid id) =>
{
    throw new NotImplementedException();
});

var marketingApi = app.MapGroup("/marketing").WithTags("Marketing");

marketingApi.MapPost("/discounts/apply", () =>
{
    throw new NotImplementedException();
});

marketingApi.MapPost("/promotions", (Guid id) =>
{
    throw new NotImplementedException();
});

var notificationsApi = app.MapGroup("/notifications").WithTags("Notifications"); ;

notificationsApi.MapPost("/{customerId}/send", (Guid customerId) =>
{
    throw new NotImplementedException();
});

notificationsApi.MapGet("/{customerId}", (Guid customerId) =>
{
    throw new NotImplementedException();
});

app.Run();