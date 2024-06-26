﻿using MovieRecommendation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MovieRecommendation.Domain.Entities
{
    public class Movie : EntityBase<Guid>
    {

        public string Title { get; set; }

        public string Summary { get; set; }

        public DateTimeOffset ReleaseDate { get; set; }

        public string Language { get; set; }

        public ICollection<Rating> Ratings { get; set; }
    }
}
