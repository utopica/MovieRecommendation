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
        Task<IEnumerable<Movie>> GetMovies(int page, int pageSize);
        Task<Movie> GetMovieDetails(string id);
        Task AddRating(string userId, string movieId, int score, string note);
        Task RecommendMovie(string userId, string movieId, string recipientEmail);
        
    }
}
