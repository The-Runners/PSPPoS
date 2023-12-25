using Domain.Enums;
using Domain.Models;

namespace Contracts;

public class OrderPostModel
{
    public Guid? CustomerId { get; init; }
}

public record OrderProductModel(Guid ProductId, int Amount);

public class OrderPatchModel
{
    public required IEnumerable<OrderProductModel> OrderProducts { get; init; }
}

public class OrderViewModel
{
    public Guid Id { get; init; }
    public Guid? CustomerId { get; init; }
    public OrderStatus Status { get; init; }

    public required IEnumerable<OrderProductViewModel> OrderProducts { get; init; }
}

public static class OrderExtensions
{
    public static OrderViewModel ToViewModel(this Order order, IEnumerable<OrderProductViewModel> orderProducts) => new()
    {
        Id = order.Id,
        CustomerId = order.CustomerId,
        Status = order.Status,
        OrderProducts = orderProducts
    };
}
