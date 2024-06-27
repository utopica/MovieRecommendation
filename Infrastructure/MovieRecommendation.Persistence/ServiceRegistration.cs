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
using MovieRecommendation.Persistence.Contexts.Identity;

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
            //services.AddScoped<IMovieService, MovieService>();

            //services.AddHttpClient<IMovieService, MovieService>();

            //var domain = configuration["Auth0:Domain"];
            //var audience = configuration["Auth0:Audience"];
            //var clientSecret = configuration["Auth0:ClientSecret"];

            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options =>
            //{
            //    options.Authority = domain;
            //    options.Audience = audience;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clientSecret))
            //    };
            //});

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        }

    }
}