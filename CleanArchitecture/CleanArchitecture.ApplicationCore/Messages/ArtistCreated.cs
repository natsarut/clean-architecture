using CleanArchitecture.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.ApplicationCore.Messages
{
    public record ArtistCreated
    {
        public required string Name { get; init; }
        public DateTime? ActiveFrom { get; init; }
        public string? EMailAddress { get; init; }
    }
}
