﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestmentCalculator.Models
{
    public class ApplicationSettings
    {
        public string Client_URL { get; set; }
        public string JWT_Secret { get; set; }
    }
}
