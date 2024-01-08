using Contracts.DTOs;
using Domain.Models;

namespace Contracts.Extensions;

public static class OrderExtensions
{
    public static OrderModelDto ToModelDto(this Order order) => new()
    {
        Id = order.Id,
        CustomerId = order.CustomerId,
        EmployeeId = order.EmployeeId,
        Status = order.Status,
        Price = order.Price,
        Discount = order.Discount,
        Tip = order.Tip,
        CreatedAt = order.CreatedAt,
    };
}