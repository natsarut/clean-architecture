using CleanArchitecture.WebUi.Shared.Models;

namespace CleanArchitecture.WebUi.Client.Code.Interfaces
{
    public interface IBffService
    {
        Task<IEnumerable<ArtistDto>> GetArtistsAsync();
    }
}
