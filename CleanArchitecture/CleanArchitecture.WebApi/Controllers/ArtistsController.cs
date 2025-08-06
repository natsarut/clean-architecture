using CleanArchitecture.ApplicationCore;
using CleanArchitecture.ApplicationCore.Entities;
using CleanArchitecture.ApplicationCore.Interfaces;
using CleanArchitecture.ApplicationCore.Models;
using CleanArchitecture.WebApi.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController(IArtistService artistService, IMapper mapper, ILogger<ArtistsController> logger) : ControllerBase
    {
        private readonly IArtistService _artistService = artistService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ArtistsController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetArtists()
        {
            _logger.LogInformation(nameof(GetArtists));
            var artists = await _artistService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ArtistDto>>(artists));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtist(Guid id)
        {
            _logger.LogInformation(nameof(GetArtist));
            Artist? artist = await _artistService.GetByIdAsync(id);
            
            if (artist == null)
            {
                _logger.LogError("Artist with ArtistId {id} not found.", id);
                return NotFound($"Artist with ArtistId {id} not found.");
            }

            return Ok(_mapper.Map<ArtistDto>(artist));
        }

        [HttpGet]
        [Route("paginated")]
        public async Task<IActionResult> GetPaginatedArtists([FromQuery] QueryStringParameters queryStringParameters)
        {
            _logger.LogInformation(nameof(GetPaginatedArtists));

            if (string.IsNullOrWhiteSpace(queryStringParameters.OrderBy))
            {
                queryStringParameters.OrderBy = nameof(ArtistDto.Name); // Default ordering by Name
            }

            PaginatedList<Artist> paginatedArtists = await _artistService.GetPaginatedAllAsync(queryStringParameters);
            var paginatedArtistsDto = new PaginatedList<ArtistDto>(_mapper.Map<IEnumerable<ArtistDto>>(paginatedArtists), paginatedArtists.TotalCount, paginatedArtists.CurrentPage, paginatedArtists.PageSize);
            Response?.Headers.Append(PaginatedList<ArtistDto>.HttpHeaderKey, paginatedArtistsDto.ToHttpHeaderValue());
            return Ok(paginatedArtistsDto);
        }

        [HttpPost]
        public async Task<IActionResult> PostArtist(ArtistDto dto)
        {
            _logger.LogInformation(nameof(PostArtist));
            Artist artist = _mapper.Map<Artist>(dto);
            await _artistService.AddAsync(artist);
            return CreatedAtAction(nameof(GetArtist), new { id = artist.ArtistId }, _mapper.Map<ArtistDto>(artist));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtist(Guid id, ArtistForUpdateDto dto)
        {
            _logger.LogInformation(nameof(PutArtist));

            if (id != dto.ArtistId)
            {
                _logger.LogError("DTO ArtistId does not match the route parameter.");
                return BadRequest();
            }

            ArtistForUpdate artistForUpdate = _mapper.Map<ArtistForUpdate>(dto);
            await _artistService.UpdateArtistAsync(artistForUpdate);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(Guid id)
        {
            _logger.LogInformation(nameof(DeleteArtist));
            await _artistService.RemoveArtistAsync(id);
            return NoContent();
        }
    }
}
