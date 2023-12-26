using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.Service
{
    public class PostService
    {
        required public string Name { get; set; }
        public string? Description { get; set; }
        required public TimeOnly Duration { get; set; }
    }
}
