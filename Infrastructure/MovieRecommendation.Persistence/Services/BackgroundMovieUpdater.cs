using Microsoft.Extensions.Hosting;
//using MovieRecommendation.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MovieRecommendation.Domain.Interfaces;

namespace MovieRecommendation.Persistence.Services
{
    public class BackgroundMovieUpdater : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _updateInterval = TimeSpan.FromHours(12);

        public BackgroundMovieUpdater(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var movieService = scope.ServiceProvider.GetRequiredService<IMovieService>();
                    await movieService.UpdateMovies();
                }

                await Task.Delay(_updateInterval, stoppingToken);
            }
        }
    }

}
