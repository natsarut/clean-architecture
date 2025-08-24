using System;
using System.Collections.Generic;

namespace CleanArchitecture.ApplicationCore.Entities;

public partial class Artist
{
    public Guid ArtistId { get; set; }
    public required string Name { get; set; }
    public DateTime? ActiveFrom { get; set; }
    public string? EMailAddress { get; set; }
    public string? MobilePhoneNumber { get; set; }
    public ICollection<Album> Albums { get; set; } = [];
}