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
        public NotificationProviders NotificationProvider { get; init; }
        public Guid ArtistId { get; init; }

        /// <summary>
        /// E-mail address or mobile phone number.
        /// </summary>
        public required string Target { get; init; } 
    }
}
