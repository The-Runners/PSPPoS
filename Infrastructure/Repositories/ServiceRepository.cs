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

    public async Task<ServiceModelDto?> GetServiceById(Guid id)
    {
        var service = await _services.FindAsync(id);
        if (service is null)
        {
            return null;
        }

        return new ServiceModelDto
        {
            Id = service.Id,
            Name = service.Name,
            Duration = service.Duration,
        };
    }

    public async Task<IEnumerable<ServiceModelDto?>> GetAllServices()
    {
        var services = await _services.ToListAsync();
        var serviceDtos = new List<ServiceModelDto>();
        foreach (var service in services)
        {
            var serviceDto = new ServiceModelDto
            {
                Id = service.Id,
                Name = service.Name,
                Duration = service.Duration,
            };
            serviceDtos.Add(serviceDto);
        }

        return serviceDtos;
    }

    public async Task AddService(ServiceCreateDto serviceDto)
    {
        var service = new Service
        {
            Id = new Guid(),
            Name = serviceDto.Name,
            Duration = serviceDto.Duration,
        };
        await _services.AddAsync(service);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateService(ServiceModelDto serviceDto)
    {
        var service = new Service
        {
            Id = serviceDto.Id,
            Name = serviceDto.Name,
            Duration = serviceDto.Duration,
        };
        _services.Update(service);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteService(Guid id)
    {
        var service = await _services.FindAsync(id);
        if (service is null)
        {
            return;
        }

        _services.Remove(service!);
        await _context.SaveChangesAsync();
    }
}