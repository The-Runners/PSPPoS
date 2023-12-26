using Contracts.DTOs.Reservation;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Employee> _reservations;
        public ReservationRepository(AppDbContext context)
        {
            _context = context;
            _reservations = _context.Set<Employee>();
        }

        public void AddReservation(PostReservation reservation)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GetReservation> GetFilteredReservations()
        {
            throw new NotImplementedException();
        }
    }
}
