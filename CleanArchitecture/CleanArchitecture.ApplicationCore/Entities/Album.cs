using System;
using System.Collections.Generic;

namespace CleanArchitecture.ApplicationCore.Entities;

public partial class Album
{
    public Guid AlbumId { get; set; }
    public required string Name { get; set; }
    public DateTime ReleasedDate { get; set; }
    public Guid ArtistId { get; set; }
    public int GenreId { get; set; }
    public Artist? Artist { get; set; }
    public Genre? Genre { get; set; }
}