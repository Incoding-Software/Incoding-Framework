namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System;
    using Incoding.Extensions;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(DateTimeExtensions))]
    public class When_date_time_extensions
    {
        It should_be_milliseconds = () => 100.Milliseconds().TotalMilliseconds.ShouldEqual(100);

        It should_be_seconds = () => 100.Seconds().TotalSeconds.ShouldEqual(100);

        It should_be_minutes = () => 100.Minutes().TotalMinutes.ShouldEqual(100);

        It should_be_hours = () => 100.Hours().TotalHours.ShouldEqual(100);

        It should_be_set_day = () => Pleasure.Generator.The20120406Noon()
                                             .SetDay(DayOfWeek.Saturday)
                                             .DayOfWeek.ShouldEqual(DayOfWeek.Saturday);

        It should_be_set_months = () => Pleasure.Generator.The20120406Noon()
                                                .SetMonth(5)
                                                .Month.ShouldEqual(5);

        It should_be_set_months_to_next_year = () => Pleasure.Generator.The20120406Noon()
                                                             .SetMonth(1)
                                                             .Year.ShouldEqual(Pleasure.Generator.The20120406Noon().Year + 1);

        It should_be_set_time = () => Pleasure.Generator.The20120406Noon()
                                              .SetTime(1, 2, 3)
                                              .ShouldBeTime(1, 2, 3);
    }
}