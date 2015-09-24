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

            // Assume after 17:00 on a business day means it will be processed the next day
            if (newDate.Hour > 17)
            {
                newDate = date.Date.AddDays(1).AddHours(9); // 9:00 AM the next day
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

        //public static List<DateTime> GetUkPublicHolidays()
        //{
        //    return new List<DateTime>()
        //    {
        //        new DateTime(2015, 12, 25),
        //        new DateTime(2015, 12, 28),
        //        new DateTime(2016, 1, 1),
        //        new DateTime(2016, 3, 25),
        //        new DateTime(2016, 3, 28),
        //        new DateTime(2016, 5, 2),
        //        new DateTime(2016, 5, 30),
        //        new DateTime(2016, 8, 29),
        //        new DateTime(2016, 12, 26),
        //        new DateTime(2016, 12, 27),
        //        new DateTime(2017, 1, 2),
        //        new DateTime(2017, 4, 14),
        //        new DateTime(2017, 4, 17),
        //        new DateTime(2017, 5, 1),
        //        new DateTime(2017, 5, 29),
        //        new DateTime(2017, 8, 28),
        //        new DateTime(2017, 12, 25),
        //        new DateTime(2017, 12, 26)
        //    }; // TODO: Add more dates!
        //}
    }
}
