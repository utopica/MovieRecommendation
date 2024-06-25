﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieRecommendation.Domain.Common;

namespace MovieRecommendation.Domain.Entities
{
    public class Recommendation : EntityBase<Guid>
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string MovieId { get; set; }
        public Movie Movie { get; set; }
        public string RecipientEmail { get; set; }
        public DateTimeOffset SentAt { get; set; }
    }
}
