using Contracts.DTOs.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IEmployeeRepository
    {
        GetEmployee GetById(Guid id);
        IEnumerable<GetEmployee> GetAll();
        void Add(PostEmployee employee);
        void Update(EditEmployee employee);

    }
}
