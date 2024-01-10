using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Interfaces;
using LanguageExt;
using WebApi.Interfaces;

namespace WebApi.Services;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _productRepository;

    public ProductService(IGenericRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> ListAsync(int offset, int limit)
    {
        return await _productRepository.ListAsync(offset, limit);
    }

    public async Task<Either<DomainException, Product>> Create(ProductCreateDto productDto)
    {
        if (productDto.Price <= 0)
        {
            return new ValidationException("The given product price is invalid.");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productDto.Name,
            Price = productDto.Price,
            Tax = productDto.Tax,
        };

        return await _productRepository.Add(product);
    }

    public async Task<Either<DomainException, Product>> GetProductById(Guid productId)
    {
        var result = await _productRepository.GetById(productId);
        return result is null ? new NotFoundException(nameof(Product), productId) : result;
    }

    public async Task<Either<DomainException, Product>> Edit(Guid productId, ProductEditDto productDto) =>
        await GetProductById(productId).BindAsync<DomainException, Product, Product>(async product =>
        {
            if (product is null)
            {
                return new NotFoundException(nameof(Product), productId);
            }

            product.Name = productDto.Name ?? product.Name;
            product.Price = productDto.Price ?? product.Price;

            return await _productRepository.Update(product);
        });

    public async Task<Either<DomainException, Unit>> Delete(Guid productId)
    {
        return await GetProductById(productId)
            .MapAsync(async _ => await _productRepository.Delete(productId))
            .Map(_ => Unit.Default);
    }
}