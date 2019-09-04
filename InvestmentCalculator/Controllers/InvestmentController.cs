using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvestmentCalculator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentController : ControllerBase
    {

        // Api for monthly expected payment
        [HttpPost]
        [Route("GetMontlyPayment")]
        public Object GetMontlyPayment(InputModel input)
        {
            decimal monthSubscription = input.subscription;
            decimal rate = input.rate;
            int tenor = input.months;
            decimal balance = input.balance;
           decimal interestEarned = calculateMonthlyInterest(monthSubscription, rate, balance);

            decimal expectedMonthly = calculateMonthlyBalance( monthSubscription,  tenor,  rate, balance);


            return new
            { 
                balance=balance,
                subscription= monthSubscription,
                interestEarned = interestEarned,
                closingBalance = expectedMonthly

            };
            
        }


        // Api for Total Investment for one Month
        [HttpPost]
        [Route("GetCompleteInvestment")]
        public IEnumerable<MonthlyOutputModel> GetCompleteInvestment(InputModel input)
        {
            List<MonthlyOutputModel> completeInvest = new List<MonthlyOutputModel>();
            decimal monthSubscription = input.subscription+0.00m;
            decimal rate = input.rate;
            int tenor = input.months;
            decimal balance = input.balance + 0.00m;
            decimal interestEarned =0.00m;

            decimal expectedMonthly = 0.00m;


            for (int i = 0; i < input.months; i++)
            {
                 
                 rate = input.rate;
                 tenor = input.months;
                 
                 interestEarned = calculateMonthlyInterest(monthSubscription, rate, balance);

                 expectedMonthly = calculateMonthlyBalance(monthSubscription, tenor, rate, balance);
              
                    MonthlyOutputModel investment = new MonthlyOutputModel(){
                         balance= Math.Round(balance, 2),
                        subscription = Math.Round(monthSubscription, 2),
                        interestEarned = Math.Round(interestEarned,2),
                        closingBalance = Math.Round(expectedMonthly,2)


                    };

                completeInvest.Add(investment);

                balance = expectedMonthly;

            }



            return completeInvest;

        }

        // Api for Total Investment for specified duration
        [HttpPost]
        [Route("GetTotalInvestment")]
        public Object GetTotalInvestment(InputModel input)
        {
            List<MonthlyOutputModel> completeInvest = new List<MonthlyOutputModel>();
            decimal monthSubscription = input.subscription;
            decimal rate = input.rate;
            int tenor = input.months;
            decimal balance = input.balance;
            decimal interestEarned = 0.00m;
            decimal expectedMonthly = 0.00m;
            decimal TotalSubscription = 0.00m;
            decimal TotalInterest = 0.00m;
             decimal TotalInvestment = 0.00m;


            for (int i = 0; i < input.months; i++)
            {

                rate = input.rate;
                tenor = input.months;

                interestEarned = calculateMonthlyInterest(monthSubscription, rate, balance);

                expectedMonthly = calculateMonthlyBalance(monthSubscription, tenor, rate, balance);

                MonthlyOutputModel investment = new MonthlyOutputModel()
                {
                    balance = balance,
                    subscription = monthSubscription,
                    interestEarned = interestEarned,
                    closingBalance = expectedMonthly


                };

                completeInvest.Add(investment);

                balance = expectedMonthly;

            }


            foreach (var element in completeInvest)
            {

                TotalSubscription+=element.subscription;
                TotalInterest += element.interestEarned;
                

            }
            TotalInvestment = TotalSubscription + TotalInterest;
            TotalOutputModel totalOutput = new TotalOutputModel()
            {
                TotalSubscription = Math.Round(TotalSubscription,2),
                TotalInterest = Math.Round(TotalInterest,2),
                TotalInvestment = Math.Round(TotalInvestment,2)

            };

            return totalOutput;

        }

        //Utility Method
        private decimal calculateMonthlyBalance(decimal monthSubscription, int tenor, decimal rate, decimal balance )
        {
            decimal interestEarned = calculateMonthlyInterest( monthSubscription,  rate,  balance);
            decimal monthlyBalance = (balance + monthSubscription + interestEarned);

            return monthlyBalance;

        }

        //Utility Method
        private decimal calculateMonthlyInterest(decimal monthSubscription,  decimal rate, decimal balance)
        {

            decimal interestEarned = (monthSubscription +balance) *((rate/100) / 12);

            return interestEarned;

        }



    }
}