using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.ApplicationCore.Models
{
    public class ArtistForUpdate
    {
        public Guid ArtistId { get; set; }
        public required string Name { get; set; }
    }
}
