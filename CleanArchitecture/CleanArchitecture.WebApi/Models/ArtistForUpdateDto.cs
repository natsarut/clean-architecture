namespace CleanArchitecture.WebApi.Models
{
    public class ArtistForUpdateDto
    {
        public Guid ArtistId { get; set; }
        public required string Name { get; set; }
    }
}
