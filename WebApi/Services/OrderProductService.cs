﻿using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Interfaces;
using WebApi.Interfaces;

namespace WebApi.Services;

public class OrderProductService : IOrderProductService
{
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IGenericRepository<Product> _productRepository;

    public OrderProductService(
        IOrderProductRepository orderProductRepository,
        IGenericRepository<Product> productRepository)
    {
        _orderProductRepository = orderProductRepository;
        _productRepository = productRepository;
    }

    public async Task<List<ProductForOrderDto>?> GenerateProductViewModels(Guid orderId)
    {
        var orderProducts = await _orderProductRepository.GetAllProductsForOrderId(orderId);
        if (orderProducts is null)
        {
            throw new NotFoundException(nameof(orderProducts), orderId);
        }

        return await GenerateModels(orderProducts);
    }

    private async Task<List<ProductForOrderDto>> GenerateModels(IEnumerable<OrderProduct> orderProducts)
    {
        var orderProductViewModels = new List<ProductForOrderDto>();
        foreach (var orderProduct in orderProducts)
        {
            var product = await _productRepository.GetById(orderProduct.ProductId);
            if (product is null)
            {
                throw new NotFoundException(nameof(product), orderProduct.ProductId);
            }

            var orderProductModel = new ProductForOrderDto
            {
                ProductId = product.Id,
                Name = product.Name,
                Amount = orderProduct.Amount,
                UnitPrice = product.Price,
                Tax = product.Tax,
            };
            orderProductViewModels.Add(orderProductModel);

        }

        return orderProductViewModels;
    }
}