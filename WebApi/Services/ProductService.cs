using Contracts.DTOs;
using Domain.Models;
using Infrastructure.Interfaces;
using WebApi.Interfaces;

namespace WebApi.Services;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _productRepository;

    public ProductService(IGenericRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> Create(ProductCreateDto productDto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productDto.Name,
            Price = productDto.Price,
        };

        return await _productRepository.Add(product);
    }

    public async Task<Product?> GetProductById(Guid productId)
    {
        return await _productRepository.GetById(productId);
    }

    public async Task<Product?> Edit(Guid productId, ProductEditDto productDto)
    {
        var productFromDb = await _productRepository.GetById(productId);
        if (productFromDb is null)
        {
            return null;
        }

        var product = new Product
        {
            Id = productId,
            Name = productDto.Name ?? productFromDb.Name,
            Price = productDto.Price ?? productFromDb.Price,
        };

        return await _productRepository.Update(product);
    }

    public async Task Delete(Guid productId)
    {
        await _productRepository.Delete(productId);
    }
}