namespace CleanArchitecture.WebUi.Shared.Models
{
    public record ArtistDto
    {
        public Guid ArtistId { get; init; }
        public required string Name { get; init; }
        public DateTime? ActiveFrom { get; init; }
    }
}
