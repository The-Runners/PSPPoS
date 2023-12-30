using Contracts.DTOs.Customer;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Filters;
using Domain.Models;
using Infrastructure.Interfaces;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly IGenericRepository<Customer> _customerRepository;

        public CustomerService(
            IGenericRepository<Employee> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> Create(CustomerCreateDto customer)
        {
            Customer customer = new()
            {
                Id = Guid.NewGuid(),
                LoyaltyDiscount = customer.LoyaltyDiscount,
                CreatedAt = DateTimeOffset.Now,
            };

            return await _customerRepository.Add(customer);
        }

        public async Task<Customer> Edit(CustomerEditDto customer)
        {
            var customer = await _customerRepository.UpdateCustomerLoyalty(customer.Id, customer.LoyaltyDiscount);
            if (customer == null)
            {
                throw new NullReferenceException("No customer found with provided Id. ");
            }

            return customer;
        }
    }
}
