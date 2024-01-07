using Contracts.DTOs;
using Domain.Models;

namespace WebApi.Interfaces;

public interface IOrderService
{
    Task AddProductsToOrder(OrderProductsDto products);

    Task RemoveProductsFromOrder(OrderProductsDto products);

    Task<decimal> CalculateOrderPrice(Order order);

    Task<OrderFinalDto> GenerateFinalOrderModel(Order order);

    Task<Order> CreateEmptyOrder(EmptyOrderCreateDto orderDto);

    Task<Order?> Edit(OrderEditDto orderDto);
}