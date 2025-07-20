using CleanArchitecture.ApplicationCore;
using CleanArchitecture.WebApi.Models;
using System.Net;
using System.Net.Http.Json;

namespace CleanArchitecture.IntegrationTests
{
    public class ArtistsControllerIntegrationTests(TestingWebApplicationFactory<Program> factory) : IClassFixture<TestingWebApplicationFactory<Program>>
    {
        // Use the factory to create an HttpClient for testing.
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task GetArtist_Returns_SingleObject()
        {
            // Arrange
            Guid id = factory.Akb48Id;

            // Act
            HttpResponseMessage response = await _client.GetAsync($"/api/artists/{id}");

            // Assert
            response.EnsureSuccessStatusCode();
            ArtistDto? returnedArtist = await response.Content.ReadFromJsonAsync<ArtistDto>();
            Assert.NotNull(returnedArtist);
            Assert.Equal(id, returnedArtist.ArtistId);
        }

        [Fact]
        public async Task GetArtist_Returns_NotFound()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // Act
            HttpResponseMessage response = await _client.GetAsync($"/api/artists/{id}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            string? content = await response.Content.ReadAsStringAsync();
            Assert.Equal($"Artist with ArtistId {id} not found.", content);
        }

        [Fact]
        public async Task GetArtists_Returns_Collection()
        {
            // Arrange

            // Act
            HttpResponseMessage response = await _client.GetAsync("/api/artists");

            // Assert
            response.EnsureSuccessStatusCode();
            List<ArtistDto>? artists = await response.Content.ReadFromJsonAsync<List<ArtistDto>>();
            Assert.NotNull(artists);
            Assert.Contains(artists, a => a.Name == TestingWebApplicationFactory<Program>.Akb48);
        }

        [Fact]
        public async Task GetPaginatedArtists_Returns_PaginatedList()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 2;

            // Act
            HttpResponseMessage response = await _client.GetAsync($"/api/artists/paginated?pageNumber={pageNumber}&pageSize={pageSize}");

            // Assert
            response.EnsureSuccessStatusCode();
            PaginatedList<ArtistDto>? paginatedArtists = await response.Content.ReadFromJsonAsync<PaginatedList<ArtistDto>>();
            Assert.NotNull(paginatedArtists);
            Assert.Equal(pageSize, paginatedArtists.Count);
        }

        [Fact]
        public async Task PostArtist_Creates_Artist()
        {
            // Arrange
            var newArtist = new ArtistDto
            {
                Name = "Test Artist",
                ActiveFrom = DateTime.UtcNow
            };

            // Act
            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/artists", newArtist);

            // Assert
            response.EnsureSuccessStatusCode();
            ArtistDto? createdArtist = await response.Content.ReadFromJsonAsync<ArtistDto>();
            Assert.NotNull(createdArtist);
            Assert.Equal(newArtist.Name, createdArtist.Name);
        }

        [Fact]
        public async Task PutArtist_Updates_Artist()
        {
            // Arrange
            Guid id = factory.TestUpdateId;
            var updatedArtist = new ArtistForUpdateDto
            {
                ArtistId = id,
                Name = "BN48"
            };

            // Act
            HttpResponseMessage response = await _client.PutAsJsonAsync($"/api/artists/{id}", updatedArtist);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Verify that the artist was updated.
            HttpResponseMessage getResponse = await _client.GetAsync($"/api/artists/{id}");
            getResponse.EnsureSuccessStatusCode();
            ArtistDto? returnedArtist = await getResponse.Content.ReadFromJsonAsync<ArtistDto>();
            Assert.NotNull(returnedArtist);
            Assert.Equal(id, returnedArtist.ArtistId);
            Assert.Equal(updatedArtist.Name, returnedArtist.Name);
        }

        [Fact]
        public async Task DeleteArtist_Removes_Artist()
        {
            // Arrange
            Guid id = factory.TestDeleteId;

            // Act
            HttpResponseMessage response = await _client.DeleteAsync($"/api/artists/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Verify that the artist was deleted.
            HttpResponseMessage getResponse = await _client.GetAsync($"/api/artists/{id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
