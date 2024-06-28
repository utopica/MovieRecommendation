using MovieRecommendation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecommendation.Domain.Interfaces
{
    public interface IMovieService
    {
        Task AddRating(string userId, string movieId, int score, string note, CancellationToken cancellationToken);
        Task RecommendMovie(string userId, string movieId, string recipientEmail, CancellationToken cancellationToken);
        
    }
}
