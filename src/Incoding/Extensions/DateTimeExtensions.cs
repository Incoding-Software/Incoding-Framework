namespace Incoding.Extensions
{
    #region << Using >>

    using System;
    using Incoding.Block;

    #endregion

    public static class DateTimeExtensions
    {
        #region Factory constructors

        public static int ToInt(this GetRecurrencyDateQuery.DayOfWeek value)
        {
            switch (value)
            {
                case GetRecurrencyDateQuery.DayOfWeek.Monday:
                    return 1;

                case GetRecurrencyDateQuery.DayOfWeek.Tuesday:
                    return 2;

                case GetRecurrencyDateQuery.DayOfWeek.Wednesday:
                    return 3;

                case GetRecurrencyDateQuery.DayOfWeek.Thursday:
                    return 4;

                case GetRecurrencyDateQuery.DayOfWeek.Friday:
                    return 5;

                case GetRecurrencyDateQuery.DayOfWeek.Saturday:
                    return 6;

                case GetRecurrencyDateQuery.DayOfWeek.Sunday:
                    return 7;
            }

            return 0;
        }

        public static int ToInt(this DayOfWeek value)
        {
            switch (value)
            {
                case DayOfWeek.Monday:
                    return 1;

                case DayOfWeek.Tuesday:
                    return 2;

                case DayOfWeek.Wednesday:
                    return 3;

                case DayOfWeek.Thursday:
                    return 4;

                case DayOfWeek.Friday:
                    return 5;

                case DayOfWeek.Saturday:
                    return 6;

                case DayOfWeek.Sunday:
                    return 7;
            }

            return 0;
        }


        public static DayOfWeek ToDayOfWeek(this int value)
        {
            switch (value)
            {
                case 1:
                    return DayOfWeek.Monday;

                case 2:
                    return DayOfWeek.Tuesday;

                case 3:
                    return DayOfWeek.Wednesday;

                case 4:
                    return DayOfWeek.Thursday;

                case 5:
                    return DayOfWeek.Friday;

                case 6:
                    return DayOfWeek.Saturday;

                case 7:
                    return DayOfWeek.Sunday;
            }

            return 0;
        }

        public static DateTime AddTimeByRepeatType(this DateTime dateTime, GetRecurrencyDateQuery.RepeatType repeatType, int value)
        {
            switch (repeatType)
            {
                case GetRecurrencyDateQuery.RepeatType.Minutely:
                    return dateTime.AddMinutes(value);

                case GetRecurrencyDateQuery.RepeatType.Hourly:
                    return dateTime.AddHours(value);

                case GetRecurrencyDateQuery.RepeatType.Daily:
                    return dateTime.AddDays(value);

                case GetRecurrencyDateQuery.RepeatType.Weekly:
                    return dateTime.AddDays(7 * value);

                case GetRecurrencyDateQuery.RepeatType.Monthly:
                    return dateTime.AddMonths(value);

                case GetRecurrencyDateQuery.RepeatType.Yearly:
                    return dateTime.AddYears(value);
            }

            return dateTime;
        }

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