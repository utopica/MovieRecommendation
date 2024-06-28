using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MovieRecommendation.Domain.Entities;
using MovieRecommendation.Domain.Interfaces;
using MovieRecommendation.Persistence.Contexts;
using MovieRecommendation.Persistence.Contexts.Identity;
using Newtonsoft.Json.Linq;

namespace MovieRecommendation.Persistence.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;
        private readonly IdentityContext _identityContext;
        private readonly IMovieEmailSender _emailSender;

        public MovieService(ApplicationDbContext context, IMovieEmailSender emailSender, IdentityContext identityContext)
        {
            _context = context;
            _emailSender = emailSender;
            _identityContext = identityContext;
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

                var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == recommendation.UserId, cancellationToken);
                var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == recommendation.MovieId);

                var reciever = recommendation.RecipientEmail;
                var fromEmail = user.Email;

                await _emailSender.SendEmail( fromEmail, reciever, movie.Title).WaitAsync(cancellationToken);


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
