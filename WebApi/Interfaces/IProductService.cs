using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using LanguageExt;

namespace WebApi.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> ListAsync(int offset, int limit);

    Task<Either<DomainException, Product>> Create(ProductCreateDto productDto);

    Task<Either<DomainException, Product>> GetProductById(Guid productId);

    Task<Either<DomainException, Product>> Edit(Guid productId, ProductEditDto productDto);

    Task Delete(Guid productId);
}