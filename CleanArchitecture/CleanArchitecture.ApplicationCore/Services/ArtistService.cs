using CleanArchitecture.ApplicationCore.Entities;
using CleanArchitecture.ApplicationCore.Exceptions;
using CleanArchitecture.ApplicationCore.Interfaces;
using CleanArchitecture.ApplicationCore.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.ApplicationCore.Services
{
    public class ArtistService(IUnitOfWork unitOfWork, ILogger<GenericService<Artist>> logger) : GenericService<Artist>(unitOfWork, logger), IArtistService
    {
        public async Task RemoveArtistAsync(Guid artistId, CancellationToken cancellationToken = default)
        {
            Artist? artist = await GetByIdAsync(artistId, cancellationToken) ?? throw new NotFoundException($"Artist with ArtistId {artistId} not found.");
            await RemoveAsync(artist, cancellationToken);
        }

        public async Task UpdateArtistAsync(ArtistForUpdate artistForUpdate, CancellationToken cancellationToken = default)
        {
            Artist? artist = await GetByIdAsync(artistForUpdate.ArtistId, cancellationToken) ?? throw new NotFoundException($"Artist with ArtistId {artistForUpdate.ArtistId} not found.");
            artist.Name = artistForUpdate.Name;
            await UpdateAsync(artist, cancellationToken);
        }
    }
}
