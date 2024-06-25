﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecommendation.Domain.Common
{
    public interface IDeletedByEntity
    {
        public DateTimeOffset? DeletedOn { get; set; }
        string? DeletedByUserId { get; set; }
        bool? IsDeleted { get; set; }
    }
}
