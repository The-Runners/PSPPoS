//using Contracts;
//using Domain.Models;
//using Infrastructure;

//namespace WebApi.Endpoints;

//public static class ProductEndpoints
//{
//    public static void MapProductEndpoints(this WebApplication app)
//    {
//        var group = app.MapGroup("product").WithTags("Products");

//        group.MapGet("{id}", (AppDbContext ctx, Guid id) =>
//        {
//            var result = ctx.Products.FirstOrDefault(x => x.Id == id);

//            if (result is null)
//            {
//                return Results.NotFound($"Product with id: `{id}` does not exist.");
//            }
//            else
//            {
//                return Results.Ok(result.ToViewModel());
//            }
//        });

//        group.MapPost(string.Empty, (AppDbContext ctx, ProductPostModel productPostModel) =>
//        {
//            var errors = new Dictionary<string, string[]>();

//            if (productPostModel.Price <= 0)
//            {
//                errors["Product price error"] = ["Product price must be positive."];
//            }

//            if (string.IsNullOrEmpty(productPostModel.Name))
//            {
//                errors["Product name error"] = ["Product name must not be null or empty."];
//            }

//            if (errors.Any())
//                return Results.ValidationProblem(errors);

//            var product = new Product()
//            {
//                Id = Guid.NewGuid(),
//                Name = productPostModel.Name,
//                Price = productPostModel.Price,
//                Description = productPostModel.Description,
//            };

//            ctx.Products.Add(product);

//            ctx.SaveChanges();

//            return Results.Ok(product.ToViewModel());
//        });
//    }
//}
