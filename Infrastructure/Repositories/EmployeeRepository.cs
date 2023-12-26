using Contracts.DTOs.Employee;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Employee> _employees;
        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
            _employees = _context.Set<Employee>();
        }

        public void Add(PostEmployee employee)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GetEmployee> GetAll()
        {
            throw new NotImplementedException();
        }

        public GetEmployee GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(EditEmployee employee)
        {
            throw new NotImplementedException();
        }
    }
}
