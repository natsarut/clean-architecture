using CleanArchitecture.WebUi.Client.Code.Interfaces;
using CleanArchitecture.WebUi.Models;
using System.Net.Http.Json;

namespace CleanArchitecture.WebUi.Client.Code.Services
{
    public class BffService(HttpClient httpClient): IBffService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<IEnumerable<ArtistDto>> GetArtistsAsync()
        {
            var response = await _httpClient.GetAsync("api/bff/getartists");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<ArtistDto>>() ?? [];
        }
    }
}
