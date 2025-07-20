using System;
using System.Collections.Generic;

namespace CA.Blocks.DataAccessTestDataForUnitTests.TestSets.DateDimension
{

    public class DateDimension
    {
        public DateDimension()
        {
        }

        private  DateTime GetLastFridayInMonth(DateTime refDate)
        {
            var lastDayOfMonth = new DateTime(refDate.Year, refDate.Month, DateTime.DaysInMonth(refDate.Year, refDate.Month));

            switch (lastDayOfMonth.DayOfWeek)
            {
                case System.DayOfWeek.Thursday:
                    lastDayOfMonth = lastDayOfMonth.AddDays(-6);
                    break;
                case System.DayOfWeek.Wednesday:
                    lastDayOfMonth = lastDayOfMonth.AddDays(-5);
                    break;
                case System.DayOfWeek.Tuesday:
                    lastDayOfMonth = lastDayOfMonth.AddDays(-4);
                    break;
                case System.DayOfWeek.Monday:
                    lastDayOfMonth = lastDayOfMonth.AddDays(-3);
                    break;
                case System.DayOfWeek.Sunday:
                    lastDayOfMonth = lastDayOfMonth.AddDays(-2);
                    break;
                case System.DayOfWeek.Saturday:
                    lastDayOfMonth = lastDayOfMonth.AddDays(-1);
                    break;
            }
            return lastDayOfMonth;
        }

        private DateTime GetFirstMondayInMonth(DateTime refDate)
        {
            var firstDayOfMonth = new DateTime(refDate.Year, refDate.Month,1 );

            switch (firstDayOfMonth.DayOfWeek)
            {
                case System.DayOfWeek.Sunday:
                    firstDayOfMonth = firstDayOfMonth.AddDays(1);
                    break;
                case System.DayOfWeek.Saturday:
                    firstDayOfMonth = firstDayOfMonth.AddDays(2);
                    break;
                case System.DayOfWeek.Friday:
                    firstDayOfMonth = firstDayOfMonth.AddDays(3);
                    break;
                case System.DayOfWeek.Thursday:
                    firstDayOfMonth = firstDayOfMonth.AddDays(4);
                    break;
                case System.DayOfWeek.Wednesday:
                    firstDayOfMonth = firstDayOfMonth.AddDays(5);
                    break;
                case System.DayOfWeek.Tuesday:
                    firstDayOfMonth = firstDayOfMonth.AddDays(6);
                    break;
            }
            return firstDayOfMonth;
        }

        private DateTime GetLastWeekDayInMonth(DateTime refDate)
        {
            var lastDayOfMonth = new DateTime(refDate.Year, refDate.Month, DateTime.DaysInMonth(refDate.Year, refDate.Month));

            switch (lastDayOfMonth.DayOfWeek)
            {
                case System.DayOfWeek.Sunday:
                    lastDayOfMonth = lastDayOfMonth.AddDays(-2);
                    break;
                case System.DayOfWeek.Saturday:
                    lastDayOfMonth = lastDayOfMonth.AddDays(-1);
                    break;
            }

            return lastDayOfMonth;
        }

        private DateTime GetFirstWeekDayInMonth(DateTime refDate)
        {
            var firstDayOfMonth = new DateTime(refDate.Year, refDate.Month,1 );

            switch (firstDayOfMonth.DayOfWeek)
            {
                case System.DayOfWeek.Sunday:
                    firstDayOfMonth = firstDayOfMonth.AddDays(1);
                    break;
                case System.DayOfWeek.Saturday:
                    firstDayOfMonth = firstDayOfMonth.AddDays(2);
                    break;
            }

            return firstDayOfMonth;
        }


        public DateDimension(DateTime date)
        {
            Date = date.Date;
            DateKey = date.ToString("yyyyMMdd");
            Year = (short)date.Year;
            Month = (byte)date.Month;
            Day = (byte)date.Day;
            DayOfWeek = (byte)date.DayOfWeek;
            DayOfYear =  (short)date.DayOfYear;

            Quarter = $"Q{((Month + 2) / 3)}";

            QuarterKey =  $"{Year}{Quarter}";

            MonthKey = $"{Year}{Month}";
            MonthShortName = date.ToString("MMM");
            MonthName = date.ToString("MMMM");

            DayName = date.ToString("dddd");

            IsFirstDayMonth = 1 == date.Day;
            IsLastDayMonth = DateTime.DaysInMonth(date.Year, date.Month) == date.Day;

            IsLastWeekdayInMonth = GetLastWeekDayInMonth(date) == date;
            IsFirstWeekDayInMonth = GetFirstWeekDayInMonth(date) == date;

            IsLastFridayInMonth = GetLastFridayInMonth(date) == date;
            IsFirstMondayInMonth = GetFirstMondayInMonth(date) == date;
        }

        public DateTime Date { get; private set; }
        public string DateKey { get; private set; }

        public short Year { get; private set; }
        public byte Month { get; private set; }
        public byte Day { get; private set; }

        public byte DayOfWeek { get; private set; } // 0 is sunday

        public short DayOfYear { get; private set; }

        public string Quarter { get; private set; } // we assume Quarter runs by physical year ir Jan Feb Mar is Q1

        public string QuarterKey { get; private set; }

        public string MonthKey { get; private set; }
        public string MonthShortName { get; private set; }
        public string MonthName { get; private set; }
        public string DayName { get; private set; }

        public bool IsFirstDayMonth { get; private set; }
        public bool IsLastDayMonth { get; private set; }
        public bool IsLastWeekdayInMonth { get; private set;}
        public bool IsFirstWeekDayInMonth { get; private set; }
        public bool IsLastFridayInMonth { get; private set; }
        public bool IsFirstMondayInMonth { get; private set; }
    }


    public class DateDimensionBuilder
    {


        public IList<DateDimension> GenerateDateDimensions(int startYear, int endYear)
        {
            var result = new List<DateDimension>();
            for(var year = startYear; year <= endYear; year++)
            {
                var date = new DateTime(year, 1, 1);
                while (date.Year == year)
                {

                    result.Add(new DateDimension(date));
                    date = date.AddDays(1);
                }
            }
            return result;
        }
    }
}
