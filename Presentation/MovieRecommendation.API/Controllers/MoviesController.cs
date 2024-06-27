using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieRecommendation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly TmdbService _tmdbService;

        public MoviesController(TmdbService tmdbService)
        {
            _tmdbService = tmdbService;
        }

        [HttpGet("popular")]
        [Authorize] 
        public async Task<IActionResult> GetPopularMovies()
        {
            try
            {
                var movies = await _tmdbService.GetPopularMoviesAsync();
                return Ok(movies);
            }
            catch (HttpRequestException ex)
            {
               
                return StatusCode(500, $"Error retrieving popular movies: {ex.Message}");
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}