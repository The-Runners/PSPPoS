using Contracts.DTOs;
using Domain.Models;

namespace WebApi.Interfaces;

public interface IProductService
{
    Task<Product> Create(ProductCreateDto productDto);

    Task<Product?> GetProductById(Guid productId);

    Task<Product?> Edit(ProductEditDto productDto);

    Task Delete(Guid productId);
}