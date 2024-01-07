using Domain.Models;
using Contracts.DTOs.Customer;

namespace WebApi.Interfaces;

public interface ICustomerService
{
    // Customer CRUD operations
    Task<Customer> Create(CustomerCreateDto customerDto);
        
    Task<Customer?> GetCustomerById(Guid customerId);
        
    Task<Customer?> Edit(CustomerEditDto customerDto);
        
    Task Delete(Guid customerId);
}