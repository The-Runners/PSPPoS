using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.Service
{
    public class EditService
    {
        required public Guid Id { get; init; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public TimeOnly? Duration { get; set; }
    }
}
