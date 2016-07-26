using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TksHelpers
{
    public static class DateExtension
    {
        /// <summary>
        /// Convert this to integer
        /// </summary>
        /// <param name="date">this date to convert</param>
        /// <returns>this as YYYYMMDD</returns>
        public static int ToInt(this DateTime date)
        {
            return (date.Year * 100 + date.Month) * 100 + date.Day;
        }

        /// <summary>
        /// Convert date to milliseconds. Asserts : 1 month == 30.4375 days. No overflow verifications
        /// </summary>
        /// <param name="dateTime">this date to convert</param>
        /// <returns>this as milliseconds</returns>
        public static long ToMillis(this DateTime dateTime)
        {
            return (long)((((((dateTime.Year * 12 + dateTime.Month - 1) * 30.4375 + dateTime.Day) * 24 + dateTime.Hour) * 60 + dateTime.Minute) * 60 + dateTime.Second) * 1000 + dateTime.Millisecond) % long.MaxValue;
        }

        /// <summary>
        /// Make a SQLite Date from this
        /// </summary>
        /// <param name="toAdapt">this to adapt</param>
        /// <returns>Adapted date</returns>
        public static string ToSqLiteFormat(this DateTime toAdapt)
        {
            return toAdapt.Year + "-" + (toAdapt.Month > 9 ? string.Empty + toAdapt.Month : "0" + toAdapt.Month) + "-" + (toAdapt.Day > 9 ? string.Empty + toAdapt.Day : "0" + toAdapt.Day);
        }
    }
}
