using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestmentCalculator.Models
{
    public class MonthlyOutputModel
    {
        public double balance { get; set; }
        public double subscription { get; set; }
        public double interestEarned { get; set; }
        public double closingBalance { get; set; }
        
    }
}
