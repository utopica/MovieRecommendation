using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Timers;
using Newtonsoft.Json;
using MovieRecommendation.Domain.Entities;
using Newtonsoft.Json.Linq;
using MovieRecommendation.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using MovieRecommendation.Domain.DTOs;

namespace MovieRecommendation.Persistence.Services
{
    public class TmdbService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly int _pageSize;
        private readonly System.Timers.Timer _updateTimer;

        public TmdbService(HttpClient httpClient, IConfiguration configuration, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
            _apiKey = configuration["Tmdb:ApiKey"];
            _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/");
            _pageSize = configuration.GetValue<int>("Tmdb:PageSize");

            _updateTimer = new System.Timers.Timer
            {
                Interval = TimeSpan.FromHours(12).TotalMilliseconds 
            };
            _updateTimer.Elapsed += async (sender, e) => await UpdateMovieDataAsync();
            _updateTimer.Start();
        }

        public async Task<string> GetPopularMoviesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"movie/popular?api_key={_apiKey}&language=en-US&page=1");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error fetching popular movies from TMDb.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching popular movies.", ex);
            }
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync(int pageNumber, int pageSize)
        {
            try
            {
                var response = await _httpClient.GetAsync($"movie/popular?api_key={_apiKey}&language=en-US&page={pageNumber}&size={pageSize}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                var movies = new List<Movie>();

                foreach (var item in json["results"])
                {
                    var title = item["title"].ToString();
                    var releaseDate = DateTimeOffset.Parse(item["release_date"].ToString()).UtcDateTime;

                    var existingMovie = await _context.Movies
                        .FirstOrDefaultAsync(m => m.Title == title && m.ReleaseDate == releaseDate);

                    if (existingMovie != null)
                    {
                        movies.Add(existingMovie);
                    }
                    else
                    {
                        var movie = new Movie
                        {
                            Id = Guid.NewGuid(),
                            Title = title,
                            Summary = item["overview"].ToString(),
                            ReleaseDate = releaseDate,
                            Language = item["original_language"].ToString(),
                            Ratings = new List<Rating>(),
                            CreatedOn = DateTimeOffset.UtcNow,
                            IsDeleted = false
                        };

                        _context.Movies.Add(movie);
                        await _context.SaveChangesAsync();

                        movies.Add(movie);
                    }
                }

                return movies;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error fetching movies from TMDb.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching movies.", ex);
            }
        }


        public async Task UpdateMovieDataAsync()
        {
            
                var pageNumber = 1;
                bool hasMoreData;

                do
                {
                    var popularMovies = await GetMoviesAsync(pageNumber, _pageSize);
                    hasMoreData = popularMovies.Any();

                    foreach (var movie in popularMovies)
                    {
                        var existingMovie = await _context.Movies
                            .FirstOrDefaultAsync(m => m.Title == movie.Title && m.ReleaseDate == movie.ReleaseDate);

                        if (existingMovie != null)
                        {
                            if (existingMovie.Summary != movie.Summary || existingMovie.Language != movie.Language ||
                                existingMovie.Title != movie.Title || existingMovie.ReleaseDate != movie.ReleaseDate)
                            {
                                existingMovie.Summary = movie.Summary;
                                existingMovie.Language = movie.Language;
                                _context.Movies.Update(existingMovie);
                            }
                        }
                        else
                        {
                            _context.Movies.Add(movie);
                        }
                    }

                    await _context.SaveChangesAsync();
                    pageNumber++;
                } while (hasMoreData);
            
        }

    

        public async Task<MovieDetailsDto> GetMovieDetails(Guid id, string userId, CancellationToken cancellationToken)
        {
            try
            {
                var movie = await _context.Movies
                    .Include(m => m.Ratings)
                    .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

                if (movie == null)
                {
                    throw new KeyNotFoundException("Movie not found.");
                }

                var tmdbAverageRating = await GetTmdbAverageRating(movie.Title, movie.ReleaseDate);

                var userRating = movie.Ratings.FirstOrDefault(r => r.UserId == Guid.Parse(userId));
                var averageRating = movie.Ratings.Any() ? movie.Ratings.Average(r => r.Score) : 0;

                return new MovieDetailsDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Summary = movie.Summary,
                    ReleaseDate = movie.ReleaseDate,
                    Language = movie.Language,
                    TmdbAverageRating = tmdbAverageRating,
                    UserRating = userRating?.Score,
                    UserNote = userRating?.Note
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching movie details.", ex);
            }
        }

        private async Task<double> GetTmdbAverageRating(string title, DateTimeOffset releaseDate)
        {
            try
            {
                var response = await _httpClient.GetAsync($"search/movie?api_key={_apiKey}&query={title}&year={releaseDate.Year}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);
                var movie = json["results"]?.FirstOrDefault();
                return movie?["vote_average"]?.ToObject<double>() ?? 0;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Error fetching TMDb average rating.", ex);
            }
        }

    }

    
}