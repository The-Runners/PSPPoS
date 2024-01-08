using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using LanguageExt;

namespace WebApi.Interfaces;

public interface IServiceService
{
    Task<Either<DomainException, IEnumerable<TimeSlot>>> GetAvailableTimeSlots(Guid serviceId,
        TimeSlot timePeriod);

    Task<Either<DomainException, Service>> GetByIdAsync(Guid serviceId);

    Task<IEnumerable<Service>> ListAsync(int offset, int limit);

    Task<Either<DomainException, Service>> AddAsync(ServiceCreateDto serviceDto);

    Task<Either<DomainException, Service>> UpdateAsync(Guid serviceId, ServiceEditDto serviceDto);

    Task Delete(Guid serviceId);
}
