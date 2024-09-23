﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using wpfPocAPI.Models.Enums;

namespace wpfPocAPI.Models
{
    public class HistogramMetric : Metric
    {
        public long ElapsedTimeMs { get; set; }
        public int[] Buckets { get; set; }
    }
}
