﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRecommendation.API.Services;
using MovieRecommendation.Domain.Interfaces;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using MovieRecommendation.API.Models;
using MovieRecommendation.Persistence.Services;
using MovieRecommendation.Domain.DTOs;

namespace MovieRecommendation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly TmdbService _tmdbService;
        private readonly IMovieService _movieService;
        private readonly IMovieEmailSender _recommendationEmailSender;

        public MoviesController(TmdbService tmdbService, IMovieService movieService, IMovieEmailSender recommendationEmailSender)
        {
            _tmdbService = tmdbService;
            _movieService = movieService;
            _recommendationEmailSender = recommendationEmailSender;
        }

        [HttpGet("popular")]
        [Authorize] 
        public async Task<IActionResult> GetPopularMovies()
        {
            try
            {
                var movies = await _tmdbService.GetPopularMoviesAsync();
                return Ok(movies);
            }
            catch (HttpRequestException ex)
            {
               
                return StatusCode(500, $"Error retrieving popular movies: {ex.Message}");
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> GetMovies([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var movies = await _tmdbService.GetMoviesAsync(pageNumber, pageSize);
                return Ok(movies);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error retrieving movies: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("{movieId}/rate")]
        [Authorize]
        public async Task<IActionResult> AddRating(Guid movieId, [FromBody] RatingModel ratingDto, CancellationToken cancellationToken)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    return Unauthorized();

                await _movieService.AddRating(userId, movieId.ToString(), ratingDto.Score, ratingDto.Note, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost("{movieId}/recommend")]
        [Authorize]
        public async Task<IActionResult> RecommendMovie(Guid movieId, [FromBody] RecommendationModel recommendMovieDto, CancellationToken cancellationToken)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    return Unauthorized();

                await _movieService.RecommendMovie(userId, movieId.ToString(), recommendMovieDto.RecipientEmail, cancellationToken);


                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        

        [HttpGet("{movieId}/GetMovieDetails")]
        [Authorize]
        public async Task<IActionResult> GetMovieDetails(Guid movieId, CancellationToken cancellationToken)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var movieDetails = await _tmdbService.GetMovieDetails(movieId, userId, cancellationToken);
                return Ok(movieDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateMovieData(CancellationToken cancellationToken)
        {
            
                await _tmdbService.UpdateMovieDataAsync();
                return Ok("Movie data updated successfully.");
            
          
        }
    }
}