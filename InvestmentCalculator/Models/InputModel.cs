using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestmentCalculator.Models
{
    public class InputModel
    {
        public double balance { get; set; }
        public double subscription { get; set; }
        public int months { get; set; }
        public double rate { get; set; }

    }
}
