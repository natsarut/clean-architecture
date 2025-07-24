using CleanArchitecture.WebUi.Models;

namespace CleanArchitecture.WebUi.Code.Services
{
    public class ArtistService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient=httpClient;

        public async Task<IEnumerable<ArtistDto>> GetArtistsAsync()
        {
            var response = await _httpClient.GetAsync("api/artists");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<ArtistDto>>() ?? [];
        }
    }
}
