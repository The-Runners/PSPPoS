using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.Reservation
{
    public class GetReservation
    {
        public Guid Id { get; init; }
        public Guid OrderId { get; init; }
        public Guid ServiceId { get; init; }
    }
}
