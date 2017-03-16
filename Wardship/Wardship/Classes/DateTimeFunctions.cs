using System;

namespace Wardship
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = startOfWeek - dt.DayOfWeek; return dt.AddDays(diff).Date;
        }
        public static DateTime StartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }
        /// <summary>
        /// Returns the first day of the current finanical year
        /// April 1st xxxx
        /// </summary>
        /// <returns></returns>
        public static DateTime StartCurrentFinancialYear()
        {
            DateTime result;

            if (DateTime.Now.Month < 4)
            {
                result = new DateTime(DateTime.Now.Year - 1, 4, 1);
            }
            else
            {
                result = new DateTime(DateTime.Now.Year, 4, 1);
            }
            return result;
        }
    }

}