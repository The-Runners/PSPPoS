using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WebApi.Endpoints;
using WebApi.Extensions;

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

builder.Services.AddDatabaseServices();
builder.Services.AddApplicationServices();

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
app.MapProductEndpoints();
app.MapCustomerEndpoints();
app.MapEmployeeEndpoints();
app.MapReservationEndpoints();
//app.MapServiceEndpoints();

using var scope = app.Services.CreateScope();
using var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
ctx.Database.Migrate();

app.Run();