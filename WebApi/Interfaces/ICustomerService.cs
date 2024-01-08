using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using LanguageExt;

namespace WebApi.Interfaces;

public interface ICustomerService
{
    Task<Either<DomainException, Customer>> AddAsync(CustomerCreateDto customerDto);

    Task<Either<DomainException, Customer>> GetByIdAsync(Guid customerId);

    Task<IEnumerable<Customer>> ListAsync(int offset, int limit);

    Task<Either<DomainException, Customer>> UpdateAsync(Guid id, CustomerUpdateDto customerUpdateDto);

    Task<Either<DomainException, Unit>> Delete(Guid customerId);
}