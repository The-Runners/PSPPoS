using Contracts.DTOs.Reservation;
using Domain.Filters;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Reservation> _reservations;
    public ReservationRepository(AppDbContext context)
    {
        _context = context;
        _reservations = _context.Set<Reservation>();
    }

    public async Task<ReservationModelDto?> GetReservationById(Guid id)
    {
        var reservation = await _reservations.FindAsync(id);
        if (reservation is null)
        {
            return null;
        }

        return new ReservationModelDto
        {
            Id = reservation.Id,
            OrderId = reservation.OrderId,
            ServiceId = reservation.ServiceId,
            TimeSlot = reservation.TimeSlot,
        };
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

    public async Task AddReservation(ReservationCreateDto reservationDto)
    {
        var reservation = new Reservation
        {
            Id = new Guid(),
            OrderId = reservationDto.OrderId,
            ServiceId = reservationDto.ServiceId,
            TimeSlot = reservationDto.TimeSlot,
        };
        await _reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
    }
}