using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using LanguageExt;

namespace WebApi.Interfaces;

public interface IReservationService
{
    Task<Either<DomainException, Reservation>> CreateReservation(ReservationOrderDto reservationOrderDto);

    Task<Either<DomainException, Order>> CancelReservation(Guid reservationId);

    Task<IEnumerable<Reservation>> ListAsync(int offset, int limit);

    Task<Either<DomainException, Reservation>> GetById(Guid reservationId);
}