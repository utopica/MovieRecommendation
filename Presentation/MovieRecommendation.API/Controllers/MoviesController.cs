using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRecommendation.Domain.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using MovieRecommendation.API.Models;

namespace MovieRecommendation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // Get a list of movies with pagination
        [HttpGet]
        public async Task<IActionResult> GetMovies([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var movies = await _movieService.GetMovies(page, pageSize);
            return Ok(movies);
        }

        // Get details of a specific movie by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieDetails(string id)
        {
            var movie = await _movieService.GetMovieDetails(id);
            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        // Add a rating for a movie
        [HttpPost("{id}/rate")]
        public async Task<IActionResult> AddRating(string id, [FromBody] RatingModel ratingModel)
        {
            var userId = User.Claims.First(c => c.Type == "sub").Value;
            await _movieService.AddRating(userId, id, ratingModel.Score, ratingModel.Note);
            return Ok();
        }

        // Recommend a movie to someone
        [HttpPost("{id}/recommend")]
        public async Task<IActionResult> RecommendMovie(string id, [FromBody] RecommendationModel recommendationModel)
        {
            var userId = User.Claims.First(c => c.Type == "sub").Value;
            await _movieService.RecommendMovie(userId, id, recommendationModel.RecipientEmail);
            return Ok();
        }
    }
}
