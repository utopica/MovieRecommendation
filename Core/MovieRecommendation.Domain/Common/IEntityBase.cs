﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecommendation.Domain.Common
{
    public interface IEntityBase<TKey>
    {
        public TKey Id { get; set; }
    }
}
