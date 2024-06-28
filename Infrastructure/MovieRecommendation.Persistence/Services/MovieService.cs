using Microsoft.EntityFrameworkCore;
using MovieRecommendation.Domain.Entities;
using MovieRecommendation.Domain.Interfaces;
using MovieRecommendation.Persistence.Contexts;
using Newtonsoft.Json.Linq;

namespace MovieRecommendation.Persistence.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddRating(string userId, string movieId, int score, string note, CancellationToken cancellationToken)
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
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding rating.", ex);
            }
        }

        public async Task RecommendMovie(string userId, string movieId, string recipientEmail, CancellationToken cancellationToken)
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
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error recommending movie.", ex);
            }
        }
    }

}
