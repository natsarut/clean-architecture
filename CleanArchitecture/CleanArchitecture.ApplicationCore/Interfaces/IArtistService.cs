using CleanArchitecture.ApplicationCore.Entities;
using CleanArchitecture.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.ApplicationCore.Interfaces
{
    public interface IArtistService:IGenericService<Artist>
    {
        Task UpdateArtistAsync(ArtistForUpdate artistForUpdate, CancellationToken cancellationToken = default);
        Task RemoveArtistAsync(Guid artistId, CancellationToken cancellationToken = default);
    }
}
