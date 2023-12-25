﻿using Domain.Enums;
using Domain.Models;

namespace Contracts;

public class OrderPostModel
{
    public Guid? CustomerId { get; init; }
}

public record OrderProductPatchModel(string Name, int Amount);

public class OrderPatchModel
{
    public Guid? CustomerId { get; init; }
    public decimal Discount { get; init; }
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
    public OrderStatus Status { get; init; }
    public decimal Discount { get; init; }
    public decimal TotalPrice { get; init; }
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
        TotalPrice = order.Price
    };
}
