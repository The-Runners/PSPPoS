using Contracts.DTOs.Order;
using Contracts.DTOs.OrderProduct;
using Domain.Models;

namespace WebApi.Interfaces;

public interface IOrderService
{
    Task<Order> CreateEmptyOrder(OrderPostModel orderDto);

    Task AddProductsToOrder(List<OrderProductCreateDto> orderProductDtos);

    //Task CalculateOrderPrice(Guid orderId);

    Task<OrderViewModel> GenerateFinalOrderModel(Guid orderId);

    Task ApplyDiscount(Guid orderId, decimal discount);

    Task AddTip(Guid orderId, decimal tip);
}