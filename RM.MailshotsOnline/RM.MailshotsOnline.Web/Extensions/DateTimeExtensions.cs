using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Web.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime AddBusinessDays(this DateTime date, int days, List<DateTime> publicHolidays)
        {
            DateTime newDate = date;

            // Assume after 14:00 on a business day means it will be processed the next day
            if (newDate.Hour > 14)
            {
                newDate = date.Date.AddDays(1).AddHours(9); // 9:00 AM the next day
                while (!newDate.IsBusinessDay(publicHolidays))
                {
                    newDate = newDate.AddDays(1);
                }
            }

            for (int day = 1; day <= days; day++)
            {
                newDate = newDate.AddDays(1);
                while (!newDate.IsBusinessDay(publicHolidays))
                {
                    newDate = newDate.AddDays(1);
                }
            }

            return newDate;
        }

        public static bool IsBusinessDay(this DateTime date, List<DateTime> publicHolidays)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            if (publicHolidays.Contains(date.Date))
            {
                return false;
            }

            return true;
        }

        public static List<DateTime> ToDateList(this string input, string separator = ",")
        {
            var result = new List<DateTime>();

            if (!string.IsNullOrEmpty(input))
            {
                var dateParts = input.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var dateString in dateParts)
                {
                    DateTime parsedDate;
                    if (DateTime.TryParse(dateString, out parsedDate))
                    {
                        result.Add(parsedDate);
                    }
                }
            }

            return result;
        }
    }
}
