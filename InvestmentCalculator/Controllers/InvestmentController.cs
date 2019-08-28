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
            double monthSubscription = input.subscription;
            double rate = input.rate;
            int tenor = input.months;
            double balance = input.balance;
           double interestEarned = calculateMonthlyInterest(monthSubscription, rate, balance);

            double expectedMonthly = calculateMonthlyBalance( monthSubscription,  tenor,  rate, balance);


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
        public Object GetCompleteInvestment(InputModel input)
        {
            List<MonthlyOutputModel> completeInvest = new List<MonthlyOutputModel>();
            double monthSubscription = input.subscription;
            double rate = input.rate;
            int tenor = input.months;
            double balance = input.balance;
            double interestEarned =0.0;

            double expectedMonthly = 0.0;


            for (int i = 0; i < input.months; i++)
            {
                 
                 rate = input.rate;
                 tenor = input.months;
                 
                 interestEarned = calculateMonthlyInterest(monthSubscription, rate, balance);

                 expectedMonthly = calculateMonthlyBalance(monthSubscription, tenor, rate, balance);
              
                    MonthlyOutputModel investment = new MonthlyOutputModel(){
                         balance= balance,
                        subscription = monthSubscription,
                        interestEarned = interestEarned,
                        closingBalance = expectedMonthly


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
            double monthSubscription = input.subscription;
            double rate = input.rate;
            int tenor = input.months;
            double balance = input.balance;
            double interestEarned = 0.0;
            double expectedMonthly = 0.0;
            double TotalSubscription = 0.0;
            double TotalInterest = 0.0;
             double TotalInvestment = 0.0;


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
                TotalSubscription = TotalSubscription,
                TotalInterest = TotalInterest,
                TotalInvestment = TotalInvestment

            };

            return totalOutput;

        }

        //Utility Method
        private double calculateMonthlyBalance(double monthSubscription, int tenor, double rate, double balance )
        {
            double interestEarned = calculateMonthlyInterest( monthSubscription,  rate,  balance);
            double monthlyBalance = Math.Round((balance + monthSubscription + interestEarned),2);

            return monthlyBalance;

        }

        //Utility Method
        private double calculateMonthlyInterest(double monthSubscription,  double rate, double balance)
        {

            double interestEarned = Math.Round((monthSubscription +balance) *(rate / 12),2);

            return interestEarned;

        }



    }
}