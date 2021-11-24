using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetSystem
{
    public class BudgetService
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            var allAmount = _budgetRepo.GetAll();
            if (start == end)
            {
                return GetAmountForOneDay(start, allAmount);
            }
            if (start < end)
            {
                var amount=0;
                var startYearMonth = new DateTime(start.Year,start.Month,1);
                var endYearMonth = new DateTime(end.Year,end.Month,1);
                
                if (startYearMonth != endYearMonth)
                {
                    // var lastDayOfStartMonth = new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));
                    // var days = (lastDayOfStartMonth - start).Days+1;
                    // amount+= days * GetAmountForOneDay(start, allAmount);
                    //
                    // var firstDayOfEndMonth = new DateTime(end.Year, end.Month, 1);
                    // days = (end - firstDayOfEndMonth).Days + 1;
                    // amount += days * GetAmountForOneDay(end, allAmount);
                
                

                    // var currentYearMonth = new DateTime(start.Year,start.Month+1,1);
                    var currentYearMonth = new DateTime(start.Year,start.Month,1);
                    // var lastSecondEndYearMonth = new DateTime(end.Year,end.Month-1,1);
                    var lastSecondEndYearMonth = new DateTime(end.Year,end.Month,1);
                
                    while (currentYearMonth<=lastSecondEndYearMonth)
                    {
                        var yearMonth = currentYearMonth.ToString("yyyyMM");
                        if (yearMonth == start.ToString("yyyyMM"))
                        {
                            var lastDayOfStartMonth = new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));
                            var days = (lastDayOfStartMonth - start).Days+1;
                            amount+= days * GetAmountForOneDay(start, allAmount);
                        }
                        else if (yearMonth == end.ToString("yyyyMM"))
                        {
                             var firstDayOfEndMonth = new DateTime(end.Year, end.Month, 1);
                             var days = (end - firstDayOfEndMonth).Days + 1; 
                             amount += days * GetAmountForOneDay(end, allAmount);
                        }
                        else
                        {
                            amount+=GetAmountForAllMonth(allAmount, yearMonth);
                        }

                        currentYearMonth=currentYearMonth.AddMonths(1);
                    }
                }
                else
                {
                    return ((end - start).Days + 1) * GetAmountForOneDay(start, allAmount);
                }
                
                return amount;
            }
            return 0;
        }

        private int GetAmountForOneDay(DateTime start, List<Budget> allAmount)
        {
            var budget = allAmount.FirstOrDefault(x => x.YearMonth.Equals(start.ToString("yyyyMM")));
            return budget == null ? 0 : budget.Amount /
                   DateTime.DaysInMonth(start.Year, start.Month);
        }

        private static int GetAmountForAllMonth(List<Budget> allAmount, string yearMonth)
        {
            var budget = allAmount.FirstOrDefault(x => x.YearMonth.Equals(yearMonth));
            return budget?.Amount ?? 0;
        }
    }
}