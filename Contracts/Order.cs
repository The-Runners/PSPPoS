using Domain.Enums;
using Domain.Models;

namespace Contracts;

public class OrderPostModel
{
    public Guid? CustomerId { get; init; }

    public required Guid EmployeeId { get; set; }

    public required decimal Price { get; set; }

    public decimal? Discount { get; set; }

    public decimal? Tip { get; set; }
}

public record OrderProductPatchModel(string Name, int Amount);

public class OrderProductItemsDto
{
    public required Guid Id { get; init; }
    public required IEnumerable<OrderProductPatchModel> OrderProducts { get; init; }
}

public class OrderPatchModel2
{
    public Guid Id { get; init; }
    public decimal Discount { get; init; }
    public decimal Tip { get; init; }
    public required IEnumerable<OrderProductPatchModel> OrderProducts { get; init; }
}

public class OrderProductViewModel
{
    public Guid ProductId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public int Amount { get; init; }
    public decimal UnitPrice { get; init; }
}

public class OrderViewModel
{
    public Guid Id { get; init; }
    public Guid? CustomerId { get; init; }
    public Guid EmployeeId { get; init; }
    public OrderStatus Status { get; init; }
    public decimal Discount { get; init; }
    public decimal TotalPrice { get; init; }
    public decimal Tip { get; init; }
    public required IEnumerable<OrderProductViewModel> OrderProducts { get; init; }
}

public static class OrderExtensions
{
    public static OrderViewModel ToViewModel(this Order order, IEnumerable<OrderProductViewModel> orderProducts) => new()
    {
        Id = order.Id,
        CustomerId = order.CustomerId,
        Status = order.Status,
        OrderProducts = orderProducts,
        Discount = order.Discount,
        TotalPrice = order.Price,
        Tip = order.Tip,
    };
}
