using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieRecommendation.Domain.Common;
using MovieRecommendation.Domain.Identity;

namespace MovieRecommendation.Domain.Entities
{
    public class Recommendation : EntityBase<Guid>
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
        public string RecipientEmail { get; set; }
        public DateTimeOffset SentAt { get; set; }
    }
}
