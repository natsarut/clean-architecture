using CleanArchitecture.WebUi.Code.Services;
using CleanArchitecture.WebUi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BffController(ArtistService artistService) : ControllerBase
    {
        private readonly ArtistService _artistService = artistService;

        [HttpGet]
        [Route("getartists")]
        public async Task<IEnumerable<ArtistDto>> GetArtistsAsync()
        {
            return await _artistService.GetArtistsAsync();
        }
    }
}
