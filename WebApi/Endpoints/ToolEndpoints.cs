using Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;

namespace WebApi.Endpoints;

public static class ToolEndpoints
{
    public static void MapToolEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/").WithTags("Tools");

        group.MapPost("/add-employee", async (
            [FromServices] IEmployeeService employeeService,
            EmployeeCreateDto employeeDto) =>
        {
            var result = await employeeService.Create(employeeDto);
            return result;
        });

        group.MapPost("/add-service", async (
            [FromServices] IServiceService serviceService,
            ServiceCreateDto serviceDto) =>
        {
            var result = await serviceService.Create(serviceDto);
            return result;
        });

        group.MapPost("/add-product", async (
            [FromServices] IProductService productService,
            ProductCreateDto productDto) =>
        {
            var result = await productService.Create(productDto);
            return result;
        });

        //group.MapPost("/add-customer", async (
        //    [FromServices] ICustomerService customerService,
        //    CustomerCreateDto customerDto) =>
        //{
        //    var result = await customerService.AddAsync(customerDto);
        //    return result.ToHttpResult();
        //});
    }
}