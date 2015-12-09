namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    /// <summary>
    ///     Query returns next date for Scheduler by provided settings.
    /// </summary>
    public class GetRecurrencyDateQuery : QueryBase<DateTime?>
    {
        protected override DateTime? ExecuteResult()
        {
            if (Type == RepeatType.Once)
                return null;

            var now = NowDate.GetValueOrDefault(DateTime.UtcNow);
            var startDate = StartDate.GetValueOrDefault(now);
            var repeatType = Type;
            int firstRepeatInterval = RepeatInterval;
            int repeatInterval = RepeatInterval;

            if (Type == RepeatType.Weekly)
            {
                if (RepeatDays.HasValue)
                {
                    bool notSetted = true;
                    var nextDayOfWeek = now.DayOfWeek;
                    var dayOfWeeks = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Where(s => RepeatDays.Value.HasFlag(s))
                                         .Select(r => r.ToInt())
                                         .OrderBy(r => r)
                                         .ToList();
                    foreach (var repeatDay in dayOfWeeks)
                    {
                        if (repeatDay >= now.DayOfWeek.ToInt())
                        {
                            nextDayOfWeek = repeatDay.ToDayOfWeek();
                            notSetted = false;
                            break;
                        }
                    }

                    if (notSetted)
                        nextDayOfWeek = dayOfWeeks[0].ToDayOfWeek();

                    int dayInterval = notSetted ? startDate.DayOfWeek.ToInt() - nextDayOfWeek.ToInt() : nextDayOfWeek.ToInt() - startDate.DayOfWeek.ToInt();

                    firstRepeatInterval = notSetted ? RepeatInterval * 7 - dayInterval : RepeatInterval * 7 + dayInterval;
                    repeatInterval = RepeatInterval * 7;
                    repeatType = RepeatType.Daily;
                }
            }

            var nextDate = startDate.AddTimeByRepeatType(repeatType, firstRepeatInterval);
            if (EndDate.HasValue)
            {
                for (;;)
                {
                    if (nextDate > EndDate)
                        return null;
                    if (nextDate > now)
                        return nextDate;

                    nextDate = nextDate.AddTimeByRepeatType(repeatType, repeatInterval);
                }
            }

            if (RepeatCount > 0)
            {
                var repeatCount = RepeatCount;
                for (int i = 1; i <= repeatCount; i++)
                {
                    if (nextDate > now)
                        return nextDate;

                    nextDate = nextDate.AddTimeByRepeatType(repeatType, repeatInterval);
                }

                return null;
            }

            for (;;)
            {
                if (nextDate > now)
                    return nextDate;

                nextDate = nextDate.AddTimeByRepeatType(repeatType, repeatInterval);
            }
        }

        #region Properties

        public DateTime? EndDate { get; set; }

        public DateTime? NowDate { get; set; }

        public int? RepeatCount { get; set; }

        public DayOfWeek? RepeatDays { get; set; }

        public int RepeatInterval { get; set; }

        public RepeatType Type { get; set; }

        public DateTime? StartDate { get; set; }

        #endregion

        #region Enums

        [Flags]
        public enum DayOfWeek
        {
            Sunday = 1,

            Monday = 2,

            Tuesday = 4,

            Wednesday = 8,

            Thursday = 16,

            Friday = 32,

            Saturday = 64,
        }

        public enum RepeatType
        {
            Once = 0,

            Minutely = 1,

            Hourly = 2,

            Daily = 3,

            Weekly = 4,

            Monthly = 5,

            Yearly = 6
        }

        #endregion
    }
}