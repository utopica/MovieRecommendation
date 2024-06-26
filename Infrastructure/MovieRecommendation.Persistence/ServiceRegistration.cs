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

namespace MovieRecommendation.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this
            IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("PostgreSQL");

            services.AddDbContext<ApplicationDbContext>(options => { options.UseNpgsql(connectionString); });

            //var domain = $"https://{configuration["Auth0:Domain"]}/";
            //var audience = configuration["Auth0:Audience"];

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options =>
            //{
            //    options.Authority = domain;
            //    options.Audience = audience;
            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("User", policy => policy.RequireClaim("scope", "read:messages"));
            //});

        }
    }
}