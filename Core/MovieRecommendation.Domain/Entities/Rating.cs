﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieRecommendation.Domain.Common;

namespace MovieRecommendation.Domain.Entities
{
    public class Rating : EntityBase<Guid>
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int Score { get; set; }  // 1-10 arası puan
        public string Note { get; set; }
    }
}
