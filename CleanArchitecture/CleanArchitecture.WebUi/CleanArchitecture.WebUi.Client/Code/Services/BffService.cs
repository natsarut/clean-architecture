using CleanArchitecture.WebUi.Client.Code.Interfaces;
using CleanArchitecture.WebUi.Shared.Models;
using System.Net.Http.Json;

namespace CleanArchitecture.WebUi.Client.Code.Services
{
    public class BffService(HttpClient http): IBffService
    {
        public async Task<IEnumerable<ArtistDto>> GetArtistsAsync()
        {
            var response = await http.GetAsync("/api/bff/getartists");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<ArtistDto>>() ?? [];
        }
    }
}
