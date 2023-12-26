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

    public async Task<ServiceModelDto?> GetById(Guid id)
    {
        var service = await _services.FindAsync(id);
        if (service == null)
        {
            return null;
        }

        return new ServiceModelDto
        {
            Id = service!.Id,
            Name = service.Name,
            Duration = service.Duration,
        };
    }

    public IEnumerable<ServiceModelDto?> GetAll()
    {
        var services = _services.ToList();
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

    public async Task Add(ServiceCreateDto serviceDto)
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

    public async Task Update(ServiceModelDto serviceDto)
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

    public async Task Delete(Guid id)
    {
        var service = await _services.FindAsync(id);
        if (service == null)
        {
            return;
        }

        _services.Remove(service!);
        await _context.SaveChangesAsync();
    }
}