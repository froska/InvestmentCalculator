using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvestmentCalculator.Models
{
    public class MonthlyOutputModel
    {
        public decimal balance { get; set; }
        public decimal subscription { get; set; }
        public decimal interestEarned { get; set; }
        public decimal closingBalance { get; set; }
        
    }
}
