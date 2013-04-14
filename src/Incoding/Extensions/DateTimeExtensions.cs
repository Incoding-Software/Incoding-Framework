namespace Incoding.Extensions
{
    #region << Using >>

    using System;

    #endregion

    public static class DateTimeExtensions
    {
        #region Factory constructors

        public static TimeSpan Hours(this int hours)
        {
            return new TimeSpan(0, hours, 0, 0, 0);
        }

        public static TimeSpan Milliseconds(this int milliseconds)
        {
            return new TimeSpan(0, 0, 0, 0, milliseconds);
        }

        public static TimeSpan Minutes(this int minutes)
        {
            return new TimeSpan(0, 0, minutes, 0, 0);
        }

        public static TimeSpan Seconds(this int seconds)
        {
            return new TimeSpan(0, 0, 0, seconds, 0);
        }

        public static DateTime SetDay(this DateTime dateTime, DayOfWeek dayOfWeek)
        {
            while (dateTime.DayOfWeek != dayOfWeek)
                dateTime = dateTime.AddDays(1);

            return dateTime;
        }

        public static DateTime SetMonth(this DateTime dateTime, int month)
        {
            while (dateTime.Month != month)
                dateTime = dateTime.AddMonths(1);

            return dateTime;
        }

        public static DateTime SetTime(this DateTime dateTime, TimeSpan time)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
        }

        public static DateTime SetTime(this DateTime dateTime, int hours, int minutes, int second)
        {
            return dateTime.SetTime(new TimeSpan(hours, minutes, second));
        }

        #endregion
    }
}