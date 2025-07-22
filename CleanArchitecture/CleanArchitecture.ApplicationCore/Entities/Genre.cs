using System;
using System.Collections.Generic;

namespace CleanArchitecture.ApplicationCore.Entities;

public partial class Genre
{
    public int GenreId { get; set; }
    public required string GenreName { get; set; }
    public ICollection<Album> Albums { get; set; } = [];
}