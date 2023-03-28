using System;
using System.Collections.Generic;

namespace CA.Blocks.DataAccessTestDataForUnitTests.TestSets.DateDimension
{

    public class DateDimension
    {
        public DateDimension()
        {
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
        }

        public DateTime Date { get; set; }
        public string DateKey { get; set; }

        public short Year { get; set; }
        public byte Month { get; set; }
        public byte Day { get; set; }

        public byte DayOfWeek { get; set; } // 0 is sunday

        public short DayOfYear { get; set; }

        public string Quarter { get; set; } // we assume Quarter runs by physical year ir Jan Feb Mar is Q1

        public string QuarterKey { get; set; }

        public string MonthKey { get; set; }
        public string MonthShortName { get; set; }
        public string MonthName { get; set; }

        public string DayName { get; set; }
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
