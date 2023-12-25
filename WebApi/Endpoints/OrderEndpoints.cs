using Contracts;
using Domain.Enums;
using Domain.Models;
using Infrastructure;

namespace WebApi.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("order").WithTags("Orders");

        group.MapGet("{id}", (AppDbContext ctx, Guid id) =>
        {
            var result = ctx.Orders.FirstOrDefault(x => x.Id == id);

            if (result is null)
            {
                return Results.NotFound($"Order with id: `{id}` does not exist.");
            }
            else
            {
                var orderProducts = ctx.OrderProducts
                    .Where(x => x.OrderId == id)
                    .Select(x => x.ToViewModel());

                return Results.Ok(result.ToViewModel(orderProducts));
            }
        });

        group.MapPost(string.Empty, (AppDbContext ctx, OrderPostModel orderPostModel) =>
        {
            var errors = new Dictionary<string, string[]>();

            if (orderPostModel.CustomerId is not null && !ctx.Customers.Any(x => x.Id == orderPostModel.CustomerId))
            {
                errors["Order CustomerId error"] = ["Order CustomerId must reference existing Customer."];
            }

            if (errors.Any())
                return Results.ValidationProblem(errors);

            var order = new Order()
            {
                Id = Guid.NewGuid(),
                Status = OrderStatus.Created,
                CustomerId = orderPostModel.CustomerId,
            };

            ctx.Orders.Add(order);

            ctx.SaveChanges();

            return Results.Ok(order.ToViewModel(Enumerable.Empty<OrderProductViewModel>()));
        });

        group.MapPatch("{orderId}", (AppDbContext ctx, Guid orderId, OrderPatchModel orderPatchModel) =>
        {
            var errors = new Dictionary<string, string[]>();

            var orderResult = ctx.Orders.FirstOrDefault(x => x.Id == orderId);

            if (orderResult is null)
                errors["Order Id error"] = [$"Order with id: `{orderId}` does not exist."];

            if (orderPatchModel.OrderProducts.Any(x => x.Amount <= 0))
            {
                errors["Order Product Amount error"] = ["Amount of products added to an order must be positive."];
            }

            if (orderPatchModel.OrderProducts.Any(x => !ctx.Products.Any(y => y.Id == x.ProductId)))
            {
                errors["Order Product Id error"] = ["Product does not exist."];
            }

            if (errors.Any())
                return Results.ValidationProblem(errors);

            var orderProducts = new List<OrderProduct>();

            foreach (var model in orderPatchModel.OrderProducts)
            {
                var orderProductResult = ctx.OrderProducts
                    .Where(x => x.OrderId == orderId)
                    .FirstOrDefault(x => x.ProductId == model.ProductId);

                if (orderProductResult is null)
                {
                    var orderProduct = new OrderProduct()
                    {
                        OrderId = orderId,
                        ProductId = model.ProductId,
                        Amount = model.Amount,
                    };

                    orderProducts.Add(orderProduct);

                    ctx.OrderProducts.Add(orderProduct);
                }
                else
                {
                    orderProductResult.Amount = model.Amount;

                    orderProducts.Add(orderProductResult);
                }
            }

            var orderProductViews = orderProducts.Select(x => x.ToViewModel());
            return Results.Ok(orderResult.ToViewModel(orderProductViews));
        });
    }
}
