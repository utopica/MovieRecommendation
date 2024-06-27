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
                Interval = TimeSpan.FromHours(12).TotalMilliseconds // 12 hours interval
            };
            //_updateTimer.Elapsed += async (sender, e) => await UpdateMovieDataAsync();
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
                    var movie = new Movie
                    {
                        Id = Guid.NewGuid(),
                        Title = item["title"].ToString(),
                        Summary = item["overview"].ToString(),
                        ReleaseDate = DateTimeOffset.Parse(item["release_date"].ToString()).UtcDateTime,
                        Language = item["original_language"].ToString(),
                        Ratings = new List<Rating>(),
                        CreatedOn = DateTimeOffset.UtcNow,
                        IsDeleted = false

                    };

                    if (!await _context.Movies.AnyAsync(m => m.Title == movie.Title && m.ReleaseDate == movie.ReleaseDate))
                    {
                        _context.Movies.Add(movie);
                        await _context.SaveChangesAsync(); 
                    }

                    movies.Add(movie);
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

       
       

        private class TmdbResponse
        {
            public List<TmdbMovie> Results { get; set; }
        }

        private class TmdbMovie
        {
            public string Title { get; set; }
            public string Overview { get; set; }
            [JsonProperty("release_date")] public string ReleaseDate { get; set; }
            [JsonProperty("original_language")] public string OriginalLanguage { get; set; }
        }
    }
}