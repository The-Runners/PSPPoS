using Domain.Models;

namespace Contracts;

public class OrderProductViewModel
{
    public Guid ProductId { get; init; }
    public Guid OrderId { get; init; }
    public int Amount { get; init; }
}

public static class OrderProductExtensions
{
    public static OrderProductViewModel ToViewModel(this OrderProduct orderProduct) => new()
    {
        OrderId = orderProduct.OrderId,
        Amount = orderProduct.Amount,
        ProductId = orderProduct.ProductId,
    };
}