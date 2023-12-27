using Domain.Models;

namespace Infrastructure.Repositories;
public class EmployeeRepository : GenericRepository<Employee>
{
    public EmployeeRepository(AppDbContext context) : base(context) { }
}