namespace CleanArchitecture.WebUi.Models
{
    public class ArtistDto
    {
        public Guid ArtistId { get; set; }
        public required string Name { get; set; }
        public DateTime? ActiveFrom { get; set; }
    }
}
