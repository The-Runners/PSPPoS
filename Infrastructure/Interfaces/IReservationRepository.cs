using Domain.Filters;
using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IReservationRepository : IGenericRepository<Reservation>
{
    Task<List<Reservation>?> GetFilteredReservations(ReservationFilter filter);

    Task<Reservation?> GetReservationByOrderId(Guid orderId);
}