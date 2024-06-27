
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MovieRecommendation.Domain.Entities;
using MovieRecommendation.Domain.Identity;

namespace MovieRecommendation.Persistence.Contexts.Identity
{
    public class IdentityContext : IdentityDbContext<User,Role,Guid>
    {
        public IdentityContext(DbContextOptions<IdentityContext> dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Ignore<Movie>();
            modelBuilder.Ignore<Recommendation>();
            modelBuilder.Ignore<Rating>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
