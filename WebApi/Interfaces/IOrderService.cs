using Contracts.DTOs.Order;
using Contracts.DTOs.OrderProduct;
using Domain.Models;

namespace WebApi.Interfaces;

public interface IOrderService
{
    Task<Order> CreateEmptyOrder(EmptyOrderCreateDto orderDto);

    Task AddProductsToOrder(OrderProductsDto products);

    Task RemoveProductsFromOrder(OrderProductsDto products);

    Task<decimal> CalculateOrderPrice(Order order);

    Task<OrderFinalDto> GenerateFinalOrderModel(Order order);

    Task ApplyOrderDiscount(Guid orderId, decimal discount);

    Task AddTip(Guid orderId, decimal tip);
}