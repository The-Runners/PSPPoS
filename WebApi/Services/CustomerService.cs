using Contracts.DTOs.Customer;
using Domain.Models;
using Infrastructure.Interfaces;
using WebApi.Interfaces;

namespace WebApi.Services;

public class CustomerService : ICustomerService
{
    private readonly IGenericRepository<Customer> _customerRepository;

    public CustomerService(IGenericRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer> Create(CustomerCreateDto customerDto)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            LoyaltyDiscount = customerDto.LoyaltyDiscount,
        };

        return await _customerRepository.Add(customer);
    }
        
    public async Task<Customer?> GetCustomerById(Guid customerId)
    {
        return await _customerRepository.GetById(customerId);
    }

    public async Task<Customer?> Edit(CustomerEditDto customerDto)
    {
        var customer = new Customer
        {
            Id = customerDto.Id,
            LoyaltyDiscount = customerDto.LoyaltyDiscount,
        };
            
        return await _customerRepository.Update(customer);
    }

    public async Task Delete(Guid customerId)
    {
        await _customerRepository.Delete(customerId);
    }
}