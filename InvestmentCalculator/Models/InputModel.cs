using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestmentCalculator.Models
{
    public class InputModel
    {
        public decimal balance { get; set; }
        public decimal subscription { get; set; }
        public int months { get; set; }
        public decimal rate { get; set; }

    }
}
