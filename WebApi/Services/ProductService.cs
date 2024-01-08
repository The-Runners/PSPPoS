﻿using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
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
        };

        return await _productRepository.Add(product);
    }

    public async Task<Either<DomainException, Product>> GetProductById(Guid productId)
    {
        var result = await _productRepository.GetById(productId);
        return result is null ? new NotFoundException(nameof(Product), productId) : result;
    }

    public async Task<Either<DomainException, Product>> Edit(Guid productId, ProductEditDto productDto)
    {
        var productFromDb = await _productRepository.GetById(productId);
        if (productFromDb is null)
        {
            return new NotFoundException(nameof(productFromDb), productId);
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