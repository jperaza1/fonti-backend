using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Common.Extensions
{
    public static class DateTimeExtension
    {
        public static List<DateTime> DatesInRange(this DateTime dateTime, DateTime limitDate)
        {
            if (limitDate.Date < dateTime.Date) return new List<DateTime>();

            var listOfDates = new List<DateTime>();

            var initialDate = dateTime;
            while (initialDate <= limitDate)
            {
                listOfDates.Add(initialDate);
                initialDate = initialDate.AddDays(1);
            }

            return listOfDates;
        }

        public static List<DateTime> WeeksInRange(this DateTime dateTime, DateTime limitDate)
        {
            if (limitDate.Date < dateTime.Date) return new List<DateTime>();

            var listOfDates = new List<DateTime>();

            var initialDate = dateTime;
            while (initialDate <= limitDate)
            {
                listOfDates.Add(initialDate);
                initialDate = initialDate.AddDays(7);
            }

            return listOfDates;
        }

        public static List<DateTime> MonthsInRange(this DateTime startDate, DateTime endDate)
        {
            if (endDate.Date < startDate.Date)
                return new List<DateTime>();

            var months = new List<DateTime>();

            // Normalizamos al primer día del mes
            var current = new DateTime(startDate.Year, startDate.Month, 1);
            var limit = new DateTime(endDate.Year, endDate.Month, 1);

            while (current <= limit)
            {
                months.Add(current);
                current = current.AddMonths(1);
            }

            return months;
        }

        public static string ToFormatDate(this DateTime value)
        {
            return value.ToString("MM/dd/yyyy");
        }

        public static string ToFormatMonthYear(this DateTime value)
        {
            return value.ToString("MMMM yyyy");
        }
    }
}
