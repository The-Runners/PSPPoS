using Contracts.DTOs.Reservation;
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

    public async Task<List<ReservationModelDto>?> GetFilteredReservations(ReservationFilter filter)
    {
        var reservations = await _reservations.ToListAsync();

        if (filter.StartDate.HasValue)
        {
            reservations = reservations.Where(r => r.TimeSlot >= filter.StartDate.Value).ToList();
        }

        if (filter.EndDate.HasValue)
        {
            reservations = reservations.Where(r => r.TimeSlot <= filter.EndDate.Value).ToList();
        }

        var reservationDtos = new List<ReservationModelDto>();
        foreach (var reservation in reservations)
        {
            var reservationDto = new ReservationModelDto
            {
                Id = reservation.Id,
                OrderId = reservation.OrderId,
                ServiceId = reservation.ServiceId,
                TimeSlot = reservation.TimeSlot,
            };
            reservationDtos.Add(reservationDto);
        }

        return reservationDtos;
    }
}