using Domain.Models;

namespace WebApi.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> Create(CustomerCreateDto customer);

        Task<Customer> Edit(CustomerEditDto customer);
    }
}
