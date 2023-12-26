using Contracts.DTOs.Reservation;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Employee> _reservations;
    public ReservationRepository(AppDbContext context)
    {
        _context = context;
        _reservations = _context.Set<Employee>();
    }

    public async Task<IEnumerable<ReservationModelDto>> GetFilteredReservations()
    {
        throw new NotImplementedException();
    }

    public async Task AddReservation(ReservationCreateDto reservation)
    {
        throw new NotImplementedException();
    }
}