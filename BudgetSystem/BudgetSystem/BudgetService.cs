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
                var budget = allAmount.FirstOrDefault(x => x.YearMonth.Equals(start.ToString("yyyyMM")));
                return budget == null ? 0 : budget.Amount /
                                            DateTime.DaysInMonth(start.Year, start.Month);
            }
            if (start < end)
            {
                var amount=0;
                var startYearMonth = new DateTime(start.Year,start.Month,1);
                var endYearMonth = new DateTime(end.Year,end.Month,1);
                
                if (startYearMonth != endYearMonth)
                {
                    var currentYearMonth = new DateTime(start.Year,start.Month,1);
                    
                    while (currentYearMonth<=endYearMonth)
                    {
                        int days;
                        if (currentYearMonth.ToString("yyyyMM") == start.ToString("yyyyMM"))
                        {
                            var overlappingStart = new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));
                            var overlappingEnd = start;
                            days = (overlappingStart - overlappingEnd).Days+1;
                        }
                        else if (currentYearMonth.ToString("yyyyMM") == end.ToString("yyyyMM"))
                        {
                             var overlappingStart = end;
                             var overlappingEnd = new DateTime(end.Year, end.Month, 1);
                             days = (overlappingStart - overlappingEnd).Days + 1; 
                        }
                        else
                        {
                            // var overlappingStart = currentYearMonth.Year;
                            // var overlappingEnd = currentYearMonth.Month;
                            // days = DateTime.DaysInMonth(overlappingStart, overlappingEnd);
                            var overlappingStart = new DateTime(currentYearMonth.Year, currentYearMonth.Month, DateTime.DaysInMonth(currentYearMonth.Year, currentYearMonth.Month));
                            var overlappingEnd = new DateTime(currentYearMonth.Year, currentYearMonth.Month, 1);
                            days = (overlappingStart - overlappingEnd).Days + 1;
                        }

                        var budget = allAmount.FirstOrDefault(x => x.YearMonth.Equals(currentYearMonth.ToString("yyyyMM")));
                        if (budget == null)
                        {
                            budget = new Budget()
                            {
                                YearMonth = currentYearMonth.ToString("yyyyMM"),
                                Amount = 0
                            };
                        }

                        amount+= days * budget.Amount / DateTime.DaysInMonth(currentYearMonth.Year, currentYearMonth.Month);
                        currentYearMonth=currentYearMonth.AddMonths(1);
                    }
                }
               
                return amount;
            }
            return 0;
        }
    }
}