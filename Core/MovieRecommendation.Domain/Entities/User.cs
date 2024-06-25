using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieRecommendation.Domain.Common;

namespace MovieRecommendation.Domain.Entities
{
    public class User : EntityBase<Guid>
    {
        public string Auth0Id { get; set; }  // Auth0 kullanıcı kimliği
        public string Email { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Recommendation> Recommendations { get; set; }
    }
}
