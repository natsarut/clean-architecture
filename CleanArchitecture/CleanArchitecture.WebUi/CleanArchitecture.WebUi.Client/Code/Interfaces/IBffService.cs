using CleanArchitecture.WebUi.Models;

namespace CleanArchitecture.WebUi.Client.Code.Interfaces
{
    public interface IBffService
    {
        Task<IEnumerable<ArtistDto>> GetArtistsAsync();
    }
}
