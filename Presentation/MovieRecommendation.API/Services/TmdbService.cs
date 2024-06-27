using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class TmdbService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public TmdbService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Tmdb:ApiKey"];
        _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/");
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
}