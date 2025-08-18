namespace CleanArchitecture.WebApi.Models
{
    public record ArtistForUpdateDto
    {
        public Guid ArtistId { get; init; }
        public required string Name { get; init; }
    }
}
