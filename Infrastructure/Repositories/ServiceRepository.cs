using Contracts.DTOs.Service;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Service> _services;
    public ServiceRepository(AppDbContext context)
    {
        _context = context;
        _services = _context.Set<Service>();
    }

    public ServiceModelDto GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ServiceModelDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task Add(ServiceCreateDto service)
    {
        throw new NotImplementedException();
    }

    public async Task Update(ServiceModelDto service)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}