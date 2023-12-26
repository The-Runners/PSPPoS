using Contracts.DTOs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IServiceRepository
    {
        GetService GetById(Guid id);
        IEnumerable<GetService> GetAll();
        void Add(PostService service);
        void Update(EditService service);
        void Delete(Guid id);

    }
}
