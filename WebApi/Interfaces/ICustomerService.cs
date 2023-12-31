using Domain.Models;
using Contracts.DTOs.Customer;

namespace WebApi.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> Create(CustomerCreateDto customer);
        
        Task<Customer?> GetCustomerById(Guid customerId);
        
        Task<Customer?> Edit(CustomerEditDto customer);
        
        Task Delete(Guid customerId);
    }
}
