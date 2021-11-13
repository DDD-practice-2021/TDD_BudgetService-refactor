﻿#region

using System;

#endregion

namespace BudgetSystem
{
    public class Budget
    {
        public int Amount { get; set; }
        public string YearMonth { get; set; }

        public Period CreatePeriod()
        {
            return new Period(FirstDay(), LastDay());
        }

        public int Days()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        public DateTime FirstDay()
        {
            return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        }

        public DateTime LastDay()
        {
            var daysInMonth = DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
            return DateTime.ParseExact(YearMonth + daysInMonth, "yyyyMMdd", null);
        }

        public decimal DailyAmount()
        {
            return Amount / (decimal)Days();
        }
    }
}