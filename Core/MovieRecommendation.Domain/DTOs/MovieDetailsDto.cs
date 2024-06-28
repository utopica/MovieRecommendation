using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecommendation.Domain.DTOs
{
    public class MovieDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public string Language { get; set; }
        public double TmdbAverageRating { get; set; }
        public int? UserRating { get; set; }
        public string UserNote { get; set; }
    }
}
