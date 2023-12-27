using AutoMapper;
using Contracts;
using Contracts.DTOs.OrderProduct;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Interfaces;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IGenericRepository<Order> orderRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<Order> CreateOrder(OrderPostModel orderDto)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = orderDto.CustomerId,
                EmployeeId = orderDto.EmployeeId,
                Status = OrderStatus.Created,
                Price = orderDto.Price,
                Discount = orderDto.Discount,
                Tip = orderDto.Tip,
            };

            return await _orderRepository.Add(order);
        }

        public async Task<OrderViewModel> AddProductsToOrder(List<OrderProductCreateDto> orderProductDtos)
        {
            var order = _orderRepository.GetById(orderProductDtos.First().OrderId);
        }
    }
}
