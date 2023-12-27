//using Contracts;
//using Domain.Enums;
//using Domain.Models;
//using Infrastructure;
//using Microsoft.EntityFrameworkCore;

//namespace WebApi.Endpoints;

//public static class OrderEndpoints
//{

//    public static void MapOrderEndpoints(this WebApplication app)
//    {
//        var group = app.MapGroup("order").WithTags("Orders");

//        group.MapGet(string.Empty, (AppDbContext ctx, int offset = 0, int limit = 100) =>
//        {
//            var orders = ctx.Orders
//                .Skip(offset)
//                .Take(limit)
//                .ToList();

//            var orderIds = orders.Select(o => o.Id).ToHashSet();

//            var orderProductLUT = ctx
//                .OrderProducts
//                .Where(x => orderIds.Contains(x.OrderId))
//                .GroupBy(x => x.OrderId)
//                .ToDictionary(x => x.Key, x => x.ToList());

//            var orderProductIds = orderProductLUT
//                .Values
//                .SelectMany(orderProducts => orderProducts.Select(x => x.ProductId))
//                .ToHashSet();

//            var productLUT = ctx
//                .Products
//                .Where(x => orderProductIds.Contains(x.Id))
//                .ToDictionary(x => x.Id);

//            var productViewModelLUT = orderProductLUT.ToDictionary(
//                kVPair => kVPair.Key,
//                kvPair => kvPair.Value.Select(orderProduct =>
//                {
//                    var product = productLUT[orderProduct.ProductId];
//                    return new OrderProductViewModel()
//                    {
//                        ProductId = product.Id,
//                        Amount = orderProduct.Amount,
//                        Description = product.Description,
//                        Name = product.Name,
//                        UnitPrice = product.Price
//                    };
//                }));

//            return Results.Ok(orders.Select(x =>
//            {
//                var productViewModels = productViewModelLUT[x.Id];
//                return x.ToViewModel(productViewModels);
//            }));
//        });

//        group.MapGet("{id}", (AppDbContext ctx, Guid id) =>
//        {
//            var result = ctx.Orders.FirstOrDefault(x => x.Id == id);

//            if (result is null)
//            {
//                return Results.NotFound($"Order with id: `{id}` does not exist.");
//            }
//            else
//            {
//                var orderProducts = ctx.OrderProducts
//                    .Where(x => x.OrderId == id)
//                    .ToList();

//                var orderProductIds = orderProducts
//                    .Select(x => x.ProductId)
//                    .ToHashSet();

//                var productLUT = ctx.Products
//                    .Where(x => orderProductIds.Contains(x.Id))
//                    .ToDictionary(x => x.Id);

//                var productViewModels = orderProducts.Select(x => new OrderProductViewModel()
//                {
//                    Amount = x.Amount,
//                    Description = productLUT[x.ProductId].Description,
//                    Name = productLUT[x.ProductId].Name,
//                    ProductId = x.ProductId,
//                    UnitPrice = productLUT[x.ProductId].Price
//                });

//                return Results.Ok(result.ToViewModel(productViewModels));
//            }
//        });

//        group.MapPost(string.Empty, (AppDbContext ctx, OrderPostModel orderPostModel) =>
//        {
//            var errors = new Dictionary<string, string[]>();

//            if (orderPostModel.CustomerId is not null && !ctx.Customers.Any(x => x.Id == orderPostModel.CustomerId))
//            {
//                errors["Order CustomerId error"] = ["Order CustomerId must reference existing Customer."];
//            }

//            if (errors.Any())
//                return Results.ValidationProblem(errors);

//            var order = new Order()
//            {
//                Id = Guid.NewGuid(),
//                Status = OrderStatus.Created,
//                CustomerId = orderPostModel.CustomerId,
//                Price = 0,
//                Discount = 0,
//                Tip = 0
//            };

//            ctx.Orders.Add(order);

//            ctx.SaveChanges();

//            return Results.Ok(order.ToViewModel(Enumerable.Empty<OrderProductViewModel>()));
//        });

//        group.MapPut("{orderId}", (AppDbContext ctx, Guid orderId, OrderPatchModel orderPatchModel) =>
//        {
//            var errors = new Dictionary<string, string[]>();

//            var orderResult = ctx.Orders.FirstOrDefault(x => x.Id == orderId);

//            if (orderResult is null)
//                errors["Order Id error"] = [$"Order with id: `{orderId}` does not exist."];

//            if (orderPatchModel.OrderProducts.Any(x => x.Amount <= 0))
//            {
//                errors["Order Product Amount error"] = ["Amount of products added to an order must be positive."];
//            }

//            if (orderPatchModel.OrderProducts.Any(x => !ctx.Products.Any(y => y.Name == x.Name)))
//            {
//                errors["Product error"] = ["Some product(s) does not exist."];
//            }

//            if (orderPatchModel.Discount is < 0 or > 1)
//            {
//                errors["Discount error"] = ["Discount value must be in range [0, 1]."];
//            }

//            if (orderPatchModel.CustomerId is not null && !ctx.Customers.Any(x => x.Id == orderPatchModel.CustomerId))
//            {
//                errors["CustomerId error"] = [$"Customer with id: `{orderPatchModel.CustomerId}` does not exist."];
//            }

//            if (orderPatchModel.Tip is < 0)
//            {
//                errors["Order Tip error"] = ["Order tip must be non-negative."];
//            }

//            if (orderResult.Status is not OrderStatus.Created)
//                errors["Order status error"] = [$"Order details can only be updated while status is not {nameof(OrderStatus.Ordered)}"];

//            if (errors.Any())
//                return Results.ValidationProblem(errors);

//            ctx.OrderProducts.Where(x => x.OrderId == orderId).ExecuteDelete();

//            var productLUT = orderPatchModel
//                .OrderProducts
//                .Select(x => ctx.Products.First(y => y.Name == x.Name))
//                .ToDictionary(x => x.Name, x => x);

//            var orderProducts = new List<OrderProduct>();
//            foreach (var model in orderPatchModel.OrderProducts)
//            {
//                var product = ctx.Products.First(x => x.Name == model.Name);
//                var orderProduct = new OrderProduct()
//                {
//                    OrderId = orderId,
//                    ProductId = productLUT[model.Name].Id,
//                    Amount = model.Amount,
//                };
//                orderProducts.Add(orderProduct);
//                ctx.OrderProducts.Add(orderProduct);
//            }

//            orderResult.CustomerId = orderPatchModel.CustomerId;
//            var customer = ctx.Customers.FirstOrDefault(x => x.Id == orderPatchModel.CustomerId);

//            // Aggregate full price of products (TODO: add services)
//            var fullPrice = orderPatchModel
//                .OrderProducts
//                .Select(x => x.Amount * productLUT[x.Name].Price)
//                .DefaultIfEmpty(0)
//                .Sum();

//            orderResult.Tip = orderPatchModel.Tip;
//            orderResult.Discount = orderPatchModel.Discount;

//            // Apply whichever discount is larger
//            var appliedDiscount = customer is null
//                ? orderPatchModel.Discount
//                : Math.Max(orderPatchModel.Discount, customer.LoyaltyDiscount);

//            // Price after discount
//            orderResult.Price = decimal.Round(orderResult.Tip + (1m - appliedDiscount) * fullPrice, 2);

//            ctx.SaveChanges();

//            var productViewModels = orderPatchModel
//                .OrderProducts
//                .Select(x => new OrderProductViewModel()
//                {
//                    Name = x.Name,
//                    ProductId = productLUT[x.Name].Id,
//                    Amount = x.Amount,
//                    Description = productLUT[x.Name].Description,
//                    UnitPrice = productLUT[x.Name].Price
//                });

//            return Results.Ok(orderResult.ToViewModel(productViewModels));
//        });
//    }
//}
