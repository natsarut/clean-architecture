using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.ApplicationCore.Entities;

namespace CleanArchitecture.Infrastructure.Databases;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.ToTable("Album");
            entity.Property(e => e.AlbumId);
            entity.Property(e => e.Name);
            entity.Property(e => e.ReleasedDate);
            entity.HasOne(d => d.Artist).WithMany(p => p.Albums).HasForeignKey(d => d.ArtistId);
            entity.HasOne(d => d.Genre).WithMany(p => p.Albums).HasForeignKey(d => d.GenreId);
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.ToTable("Artist");
            entity.Property(e => e.ArtistId).ValueGeneratedNever();
            entity.Property(e => e.ActiveFrom);
            entity.Property(e => e.Name);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Genre");
            entity.Property(e => e.GenreId);
            entity.Property(e => e.GenreName);
        });
    }
}