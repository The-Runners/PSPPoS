using Domain.Filters;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    private readonly DbSet<Reservation> _reservations;

    public ReservationRepository(AppDbContext context) : base(context) 
    { 
        _reservations = context.Set<Reservation>();
    }

    public async Task<List<Reservation>?> GetFilteredReservations(ReservationFilter filter)
    {
        var reservations = await _reservations.ToListAsync();

        if (filter.StartDate.HasValue)
        {
            reservations = reservations.Where(r => r.StartTime >= filter.StartDate.Value).ToList();
        }

        if (filter.EndDate.HasValue)
        {
            reservations = reservations.Where(r => r.StartTime <= filter.EndDate.Value).ToList();
        }

        if (filter.OrderId.HasValue) 
        { 
            reservations = reservations.Where(r => r.OrderId ==  filter.OrderId).ToList();
        }

        return reservations;
    }

    public async Task<Reservation?> GetReservationByOrderId(Guid orderId)
    {
        return await _reservations.FindAsync(orderId);
    }
}