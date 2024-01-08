using Contracts.DTOs;
using Domain.Models;

namespace Contracts.Extensions;

public static class ServiceExtensions
{
    public static ServiceModelDto ToModelDto(this Service service) => new()
    {
        Id = service.Id,
        Name = service.Name,
        Duration = service.Duration,
        Price = service.Price,
    };
}