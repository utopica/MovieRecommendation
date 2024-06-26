using Microsoft.EntityFrameworkCore;
using MovieRecommendation.Domain.Entities;
using MovieRecommendation.Domain.Interfaces;
using MovieRecommendation.Persistence.Contexts;

namespace MovieRecommendation.Persistence.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetMovies(int page, int pageSize)
        {
            return await _context.Movies
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Movie> GetMovieDetails(string id)
        {
            return await _context.Movies
                .Include(m => m.Ratings)
                .FirstOrDefaultAsync(m => m.Id == Guid.Parse(id));
        }

        public async Task AddRating(string userId, string movieId, int score, string note)
        {
            var rating = new Rating
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(userId),
                MovieId = Guid.Parse(movieId),
                Score = score,
                Note = note,
                CreatedOn = DateTimeOffset.UtcNow,
                IsDeleted = false,
            };
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
        }

        public async Task RecommendMovie(string userId, string movieId, string recipientEmail)
        {
            var recommendation = new Recommendation
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(userId),
                MovieId = Guid.Parse(movieId),
                RecipientEmail = recipientEmail,
                SentAt = DateTime.UtcNow,
                CreatedOn = DateTimeOffset.UtcNow,
            };
            _context.Recommendations.Add(recommendation);
            await _context.SaveChangesAsync();

            // E-posta gönderme işlemi burada yapılabilir
        }

        public async Task UpdateMovies()
        {
            // TMDB API'dan verileri çekme ve güncelleme işlemleri burada yapılır
        }
    }

}
