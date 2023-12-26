using Contracts.DTOs.Service;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Service> _services;
        public ServiceRepository(AppDbContext context)
        {
            _context = context;
            _services = _context.Set<Service>();
        }

        public void Add(PostService service)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GetService> GetAll()
        {
            throw new NotImplementedException();
        }

        public GetService GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(EditService service)
        {
            throw new NotImplementedException();
        }
    }
}
