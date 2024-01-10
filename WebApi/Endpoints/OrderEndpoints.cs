using Contracts.DTOs;
using Contracts.DTOs.Order;
using Contracts.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;

namespace WebApi.Endpoints;

public static class OrderEndpoints
{

    public static void MapOrderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("order").WithTags("Orders");

        group.MapGet(string.Empty, GetOrdersAsync);

        group.MapGet("{id:guid}", async ([FromServices] IOrderService service, Guid id) =>
        {
            var order = await service.GetByIdAsync(id);
            var model = await service.GenerateFinalOrderModel(order);
            return Results.Ok(model);
        });

        group.MapPost(string.Empty, async (
            [FromServices] IOrderService service,
            EmptyOrderCreateDto orderPostModel) =>
        {
            var order = await service.CreateEmptyOrder(orderPostModel);
            return Results.Ok(order.ToModelDto());
        });

        group.MapPut("{id:guid}", async (
            [FromServices] IOrderService service,
            Guid id,
            OrderUpdateDto orderUpdateDto) =>
        {
            var model = await service.UpdateOrderAsync(id, orderUpdateDto);
            return Results.Ok(model);
        });
    }

    private static async Task<IResult> GetOrdersAsync(
        [FromServices] IOrderService service,
        int offset = 0,
        int limit = 100)
    {
        var orders = await service.GetAsync(offset, limit);
        return Results.Ok(orders.Select(x => x.ToModelDto()));
    }
}