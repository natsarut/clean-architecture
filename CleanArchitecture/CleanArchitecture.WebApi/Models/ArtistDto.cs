namespace CleanArchitecture.WebApi.Models
{
    public record ArtistDto
    {
        public Guid ArtistId { get; init; }
        public required string Name { get; init; }
        public DateTime? ActiveFrom { get; init; }
        public string? EMailAddress { get; init; }
    }
}
