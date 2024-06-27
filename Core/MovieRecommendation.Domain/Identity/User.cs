using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieRecommendation.Domain.Common;
using MovieRecommendation.Domain.Entities;

namespace MovieRecommendation.Domain.Identity

{
    public class User :IdentityUser<Guid>, IEntityBase<Guid>, ICreatedByEntity, IDeletedByEntity, IModifiedByEntity
    {
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Recommendation> Recommendations { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string? CreatedByUserId { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public string? ModifiedByUserId { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public string? DeletedByUserId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
