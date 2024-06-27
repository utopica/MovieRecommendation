using Microsoft.EntityFrameworkCore;
using MovieRecommendation.Domain.Entities;
using MovieRecommendation.Domain.Interfaces;
using MovieRecommendation.Persistence.Contexts;

namespace MovieRecommendation.Persistence.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;
        private readonly TmdbService _tmdbService;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Movie>> GetMovies(int page, int pageSize)
        {
            try
            {
                var movies = await _tmdbService.GetMoviesAsync(page, pageSize);

                // Save movies to the database
                foreach (var movie in movies)
                {
                    if (!await _context.Movies.AnyAsync(m => m.Id == movie.Id))
                    {
                        _context.Movies.Add(movie);
                    }
                }

                await _context.SaveChangesAsync();

                // Return movies from the database
                return await _context.Movies
                    .OrderBy(m => m.Title)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching movies.", ex);
            }
        }

        public async Task<Movie> GetMovieDetails(string id)
        {
            try
            {
                var movie = await _context.Movies
                    .Include(m => m.Ratings)
                    .FirstOrDefaultAsync(m => m.Id == Guid.Parse(id));

                if (movie == null)
                {
                    throw new KeyNotFoundException("Movie not found.");
                }

                return movie;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching movie details.", ex);
            }
        }
        public async Task AddRating(string userId, string movieId, int score, string note)
        {
            try
            {
                var rating = new Rating
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Parse(userId),
                    MovieId = Guid.Parse(movieId),
                    Score = score,
                    Note = note,
                    CreatedOn = DateTimeOffset.UtcNow,
                    IsDeleted = false
                };

                _context.Ratings.Add(rating);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding rating.", ex);
            }
        }

        public async Task RecommendMovie(string userId, string movieId, string recipientEmail)
        {
            try
            {
                var recommendation = new Recommendation
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Parse(userId),
                    MovieId = Guid.Parse(movieId),
                    RecipientEmail = recipientEmail,
                    SentAt = DateTimeOffset.UtcNow,
                    CreatedOn = DateTimeOffset.UtcNow,
                    IsDeleted = false
                };

                _context.Recommendations.Add(recommendation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error recommending movie.", ex);
            }
        }
    }

}
