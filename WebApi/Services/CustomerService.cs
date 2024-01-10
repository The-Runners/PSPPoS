using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Interfaces;
using LanguageExt;
using WebApi.Interfaces;

namespace WebApi.Services;

public class CustomerService : ICustomerService
{
    private readonly IGenericRepository<Customer> _customerRepository;

    public CustomerService(IGenericRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Either<DomainException, Customer>> AddAsync(CustomerCreateDto customerCreateDto)
    {
        if (customerCreateDto.LoyaltyDiscount is < 0 or > 1)
            return new ValidationException("Loyalty discount out of range [0, 1].");

        return await _customerRepository.Add(new Customer
        {
            Id = Guid.NewGuid(),
            LoyaltyDiscount = customerCreateDto.LoyaltyDiscount,
            CreatedAt = DateTimeOffset.UtcNow,
        });
    }

    public async Task<Either<DomainException, Customer>> GetByIdAsync(Guid customerId)
    {
        var result = await _customerRepository.GetById(customerId);

        return result is null
            ? new NotFoundException(nameof(Customer), customerId)
            : result;
    }

    public async Task<IEnumerable<Customer>> ListAsync(int offset, int limit)
    {
        return await _customerRepository.ListAsync(offset, limit);
    }

    public async Task<Either<DomainException, Customer>> UpdateAsync(Guid id, CustomerUpdateDto customerUpdateDto) =>
        await GetByIdAsync(id).BindAsync<DomainException, Customer, Customer>(async customer =>
        {
            customer.LoyaltyDiscount = customerUpdateDto.LoyaltyDiscount;

            if (customerUpdateDto.LoyaltyDiscount is < 0 or > 1)
                return new ValidationException("Loyalty discount out of range [0, 1].");

            await _customerRepository.SaveChangesAsync();

            return customer;
        });

    public async Task<Either<DomainException, Unit>> Delete(Guid id) =>
        await GetByIdAsync(id)
        .MapAsync(async _ => await _customerRepository.Delete(id))
        .Map(_ => Unit.Default);
}