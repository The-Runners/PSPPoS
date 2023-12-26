using Contracts.DTOs.Employee;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Employee> _employees;
    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
        _employees = _context.Set<Employee>();
    }

    public EmployeeModelDto GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<EmployeeModelDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task Add(EmployeeCreateDto employee)
    {
        throw new NotImplementedException();
    }

    public async Task Update(EmployeeModelDto employee)
    {
        throw new NotImplementedException();
    }
}