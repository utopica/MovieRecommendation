using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieRecommendation.Domain.Entities;

namespace MovieRecommendation.Persistence.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            // Properties
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).ValueGeneratedOnAdd();

            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.Summary)
                .HasMaxLength(1000);

            //builder.Property(m => m.Director)
            //    .HasMaxLength(100);

            builder.Property(m => m.ReleaseDate)
                .IsRequired();

            builder.Property(m => m.Language)
                .IsRequired();

            //builder.Property(m => m.Budget)
            //    .IsRequired();

            //builder.Property(m => m.Revenue)
            //    .IsRequired();

            builder.HasMany(m => m.Ratings)
                .WithOne(r => r.Movie)
                .HasForeignKey(r => r.MovieId);

            // COMMON FIELDS

            // CreatedByUserId
            builder.Property(x => x.CreatedByUserId).IsRequired(false);
            builder.Property(x => x.CreatedByUserId).HasMaxLength(75);

            // CreatedOn
            builder.Property(x => x.CreatedOn).IsRequired();

            // ModifiedByUserId
            builder.Property(x => x.ModifiedByUserId).IsRequired(false);
            builder.Property(x => x.ModifiedByUserId).HasMaxLength(75);

            // LastModifiedOn
            builder.Property(x => x.ModifiedOn).IsRequired(false);

            // DeletedByUserId
            builder.Property(x => x.DeletedByUserId).IsRequired(false);
            builder.Property(x => x.DeletedByUserId).HasMaxLength(75);

            // DeletedOn
            builder.Property(x => x.DeletedOn).IsRequired(false);

            // IsDeleted
            builder.Property(x => x.IsDeleted).IsRequired();

            builder.ToTable("Movies");

        }
    }
}
