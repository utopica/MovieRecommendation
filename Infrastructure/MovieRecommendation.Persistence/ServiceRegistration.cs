using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieRecommendation.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MovieRecommendation.Domain.Identity;
using MovieRecommendation.Domain.Interfaces;
using MovieRecommendation.Persistence.Contexts.Identity;
using MovieRecommendation.Persistence.Services;

namespace MovieRecommendation.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this
            IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("PostgreSQL");

            services.AddDbContext<ApplicationDbContext>(options => { options.UseNpgsql(connectionString); });

            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IMovieService, MovieService>();

        }

    }
}