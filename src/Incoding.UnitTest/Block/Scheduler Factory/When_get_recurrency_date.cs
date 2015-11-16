namespace IncodingContrib.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.Block;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetRecurrencyDateQuery))]
    public class When_get_recurrency_date
    {
        #region Establish value

        static DateTime now = DateTime.Now;

        static MockMessage<GetRecurrencyDateQuery, DateTime?> Execute(Action<IInventFactoryDsl<GetRecurrencyDateQuery>> dsl)
        {
            var query = Pleasure.Generator.Invent(dsl);

            var res = MockQuery<GetRecurrencyDateQuery, DateTime?>
                    .When(query);
            res.Original.Execute();
            return res;
        }

        #endregion

        It should_be_once = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Once))
                                          .ShouldBeIsResult((DateTime?)null);

        It should_be_hollow_recurrency_date_daily_with_end_date = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Daily)
                                                                                          .Tuning(r => r.RepeatInterval, 5)
                                                                                          .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                          .Tuning(r => r.EndDate, Convert.ToDateTime("05/20/2015 17:00"))
                                                                                          .Tuning(r => r.NowDate, Convert.ToDateTime("05/20/2015 17:01")))
                                                                                .ShouldBeIsResult((DateTime?)null);

        It should_be_hollow_recurrency_date_daily_with_repeat_count = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Daily)
                                                                                              .Tuning(r => r.RepeatInterval, 1)
                                                                                              .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                              .Tuning(r => r.EndDate, null)
                                                                                              .Tuning(r => r.RepeatCount, 2)
                                                                                              .Tuning(r => r.NowDate, Convert.ToDateTime("05/18/2015 16:39")))
                                                                                    .ShouldBeIsResult((DateTime?)null);

        It should_be_hollow_recurrency_date_hourly_with_end_date = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Hourly)
                                                                                           .Tuning(r => r.RepeatInterval, 5)
                                                                                           .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                           .Tuning(r => r.EndDate, Convert.ToDateTime("05/15/2015 22:00"))
                                                                                           .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2015 23:00")))
                                                                                 .ShouldBeIsResult((DateTime?)null);

        It should_be_hollow_recurrency_date_hourly_with_repeat_count = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Hourly)
                                                                                               .Tuning(r => r.RepeatInterval, 1)
                                                                                               .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                               .Tuning(r => r.EndDate, null)
                                                                                               .Tuning(r => r.RepeatCount, 2)
                                                                                               .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2015 18:39")))
                                                                                     .ShouldBeIsResult((DateTime?)null);

        It should_be_get_hollow_recurrency_date_minutely_with_end_date = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Minutely)
                                                                                                 .Tuning(r => r.RepeatInterval, 5)
                                                                                                 .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                                 .Tuning(r => r.EndDate, Convert.ToDateTime("05/15/2015 16:41"))
                                                                                                 .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2015 16:42")))
                                                                                       .ShouldBeIsResult((DateTime?)null);

        It should_be_get_hollow_recurrency_date_minutely_with_repeat_count = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Minutely)
                                                                                                     .Tuning(r => r.RepeatInterval, 1)
                                                                                                     .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                                     .Tuning(r => r.EndDate, null)
                                                                                                     .Tuning(r => r.RepeatCount, 2)
                                                                                                     .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2015 16:39")))
                                                                                           .ShouldBeIsResult((DateTime?)null);

        It should_be_get_hollow_recurrency_date_monthly_with_end_date = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Monthly)
                                                                                                .Tuning(r => r.RepeatInterval, 2)
                                                                                                .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                .Tuning(r => r.EndDate, Convert.ToDateTime("08/14/2015 17:00"))
                                                                                                .Tuning(r => r.NowDate, Convert.ToDateTime("07/15/2015 16:01")))
                                                                                      .ShouldBeIsResult((DateTime?)null);

        It should_be_get_recurrency_date_weekly_with_end_date_and_repeat_days_second = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                                               .Tuning(r => r.RepeatDays, GetRecurrencyDateQuery.DayOfWeek.Tuesday | GetRecurrencyDateQuery.DayOfWeek.Sunday)
                                                                                                               .Tuning(r => r.RepeatInterval, 1)
                                                                                                               .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                               .Tuning(r => r.EndDate, Convert.ToDateTime("06/07/2015 16:00"))
                                                                                                               .Tuning(r => r.RepeatCount, null)
                                                                                                               .Tuning(r => r.NowDate, Convert.ToDateTime("05/25/2015 16:00")))
                                                                                                     .ShouldBeIsResult(Convert.ToDateTime("05/26/2015 16:00"));

        It should_be_get_recurrency_date_weekly_with_endless_and_repeat_days_first = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                                             .Tuning(r => r.RepeatDays, GetRecurrencyDateQuery.DayOfWeek.Tuesday | GetRecurrencyDateQuery.DayOfWeek.Thursday)
                                                                                                             .Tuning(r => r.RepeatInterval, 2)
                                                                                                             .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                             .Tuning(r => r.EndDate, null)
                                                                                                             .Tuning(r => r.RepeatCount, null)
                                                                                                             .Tuning(r => r.NowDate, Convert.ToDateTime("05/16/2015 16:00")))
                                                                                                   .ShouldBeIsResult(Convert.ToDateTime("5/26/2015 4:00:00 PM"));

        It should_be_get_recurrency_date_weekly_with_endless_and_repeat_days_second = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                                              .Tuning(r => r.RepeatDays, GetRecurrencyDateQuery.DayOfWeek.Tuesday | GetRecurrencyDateQuery.DayOfWeek.Sunday)
                                                                                                              .Tuning(r => r.RepeatInterval, 2)
                                                                                                              .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                              .Tuning(r => r.EndDate, null)
                                                                                                              .Tuning(r => r.RepeatCount, null)
                                                                                                              .Tuning(r => r.NowDate, Convert.ToDateTime("05/16/2015 16:00")))
                                                                                                    .ShouldBeIsResult(Convert.ToDateTime("5/31/2015 4:00:00 PM"));

        It should_be_get_hollow_recurrency_date_monthly_with_repeat_count = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Monthly)
                                                                                                    .Tuning(r => r.RepeatInterval, 1)
                                                                                                    .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                                    .Tuning(r => r.EndDate, null)
                                                                                                    .Tuning(r => r.RepeatCount, 2)
                                                                                                    .Tuning(r => r.NowDate, Convert.ToDateTime("07/18/2015 16:39")))
                                                                                          .ShouldBeIsResult((DateTime?)null);

        It should_be_get_hollow_recurrency_date_weekly_with_end_date = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                               .Tuning(r => r.RepeatInterval, 2)
                                                                                               .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                               .Tuning(r => r.EndDate, Convert.ToDateTime("06/11/2015 16:00"))
                                                                                               .Tuning(r => r.RepeatCount, null)
                                                                                               .Tuning(r => r.RepeatDays, null)
                                                                                               .Tuning(r => r.NowDate, Convert.ToDateTime("05/30/2015 16:00")))
                                                                                     .ShouldBeIsResult((DateTime?)null);

        It should_be_get_hollow_recurrency_date_weekly_with_end_date_and_days_first = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                                              .Tuning(r => r.RepeatInterval, 2)
                                                                                                              .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                              .Tuning(r => r.EndDate, Convert.ToDateTime("05/30/2015 23:00"))
                                                                                                              .Tuning(r => r.RepeatCount, null)
                                                                                                              .Tuning(r => r.RepeatDays, GetRecurrencyDateQuery.DayOfWeek.Monday | GetRecurrencyDateQuery.DayOfWeek.Sunday)
                                                                                                              .Tuning(r => r.NowDate, Convert.ToDateTime("05/30/2015 16:00")))
                                                                                                    .ShouldBeIsResult((DateTime?)null);

        It should_be_get_hollow_recurrency_date_weekly_with_end_date_and_days_second = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                                               .Tuning(r => r.RepeatInterval, 2)
                                                                                                               .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                               .Tuning(r => r.EndDate, Convert.ToDateTime("06/07/2015 23:00"))
                                                                                                               .Tuning(r => r.RepeatCount, null)
                                                                                                               .Tuning(r => r.RepeatDays, GetRecurrencyDateQuery.DayOfWeek.Monday | GetRecurrencyDateQuery.DayOfWeek.Friday)
                                                                                                               .Tuning(r => r.NowDate, Convert.ToDateTime("05/30/2015 16:00")))
                                                                                                     .ShouldBeIsResult((DateTime?)null);

        It should_be_get_hollow_recurrency_date_weekly_with_repeat_count = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                                   .Tuning(r => r.RepeatInterval, 2)
                                                                                                   .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                   .Tuning(r => r.EndDate, null)
                                                                                                   .Tuning(r => r.RepeatCount, 1)
                                                                                                   .Tuning(r => r.RepeatDays, null)
                                                                                                   .Tuning(r => r.NowDate, Convert.ToDateTime("05/30/2015 16:00")))
                                                                                         .ShouldBeIsResult((DateTime?)null);

        It should_be_get_hollow_recurrency_date_weekly_with_repeat_count_and_days_first = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                                                  .Tuning(r => r.RepeatInterval, 2)
                                                                                                                  .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                                  .Tuning(r => r.EndDate, null)
                                                                                                                  .Tuning(r => r.RepeatCount, 1)
                                                                                                                  .Tuning(r => r.RepeatDays, GetRecurrencyDateQuery.DayOfWeek.Monday | GetRecurrencyDateQuery.DayOfWeek.Sunday)
                                                                                                                  .Tuning(r => r.NowDate, Convert.ToDateTime("05/31/2015 16:01")))
                                                                                                        .ShouldBeIsResult((DateTime?)null);

        It should_be_get_hollow_recurrency_date_weekly_with_repeat_count_and_days_second = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                                                   .Tuning(r => r.RepeatInterval, 2)
                                                                                                                   .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                                   .Tuning(r => r.EndDate, null)
                                                                                                                   .Tuning(r => r.RepeatCount, 1)
                                                                                                                   .Tuning(r => r.RepeatDays, GetRecurrencyDateQuery.DayOfWeek.Monday | GetRecurrencyDateQuery.DayOfWeek.Tuesday)
                                                                                                                   .Tuning(r => r.NowDate, Convert.ToDateTime("05/30/2015 16:00")))
                                                                                                         .ShouldBeIsResult((DateTime?)null);

        It should_be_get_hollow_recurrency_date_yearly_with_end_date = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Yearly)
                                                                                               .Tuning(r => r.RepeatInterval, 5)
                                                                                               .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                               .Tuning(r => r.EndDate, Convert.ToDateTime("05/15/2020 16:41"))
                                                                                               .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2020 16:42")))
                                                                                     .ShouldBeIsResult((DateTime?)null);

        It should_be_get_hollow_recurrency_date_yearly_with_repeat_count = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Yearly)
                                                                                                   .Tuning(r => r.RepeatInterval, 1)
                                                                                                   .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                                   .Tuning(r => r.EndDate, null)
                                                                                                   .Tuning(r => r.RepeatCount, 2)
                                                                                                   .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2017 16:39")))
                                                                                         .ShouldBeIsResult((DateTime?)null);

        It should_be_get_recurrency_date_daily_with_end_date = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Daily)
                                                                                       .Tuning(r => r.RepeatInterval, 5)
                                                                                       .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                       .Tuning(r => r.EndDate, Convert.ToDateTime("05/20/2015 20:23"))
                                                                                       .Tuning(r => r.NowDate, Convert.ToDateTime("05/19/2015 16:37")))
                                                                             .ShouldBeIsResult(Convert.ToDateTime("05/20/2015 16:36"));

        It should_be_get_recurrency_date_daily_with_endless = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Daily)
                                                                                      .Tuning(r => r.RepeatInterval, 5)
                                                                                      .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                      .Tuning(r => r.EndDate, null)
                                                                                      .Tuning(r => r.RepeatCount, null)
                                                                                      .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2015 16:42")))
                                                                            .ShouldBeIsResult(Convert.ToDateTime("05/20/2015 16:36"));

        It should_be_get_recurrency_date_daily_with_repeat_count = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Daily)
                                                                                           .Tuning(r => r.RepeatInterval, 5)
                                                                                           .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                           .Tuning(r => r.EndDate, null)
                                                                                           .Tuning(r => r.RepeatCount, 3)
                                                                                           .Tuning(r => r.NowDate, Convert.ToDateTime("05/19/2015 16:38")))
                                                                                 .ShouldBeIsResult(Convert.ToDateTime("05/20/2015 16:36"));

        It should_be_get_recurrency_date_hourly_with_end_date = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Hourly)
                                                                                        .Tuning(r => r.RepeatInterval, 5)
                                                                                        .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 14:36"))
                                                                                        .Tuning(r => r.EndDate, Convert.ToDateTime("05/15/2015 20:23"))
                                                                                        .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2015 19:30")))
                                                                              .ShouldBeIsResult(Convert.ToDateTime("05/15/2015 19:36"));

        It should_be_get_recurrency_date_hourly_with_endless = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Hourly)
                                                                                       .Tuning(r => r.RepeatInterval, 5)
                                                                                       .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                       .Tuning(r => r.EndDate, null)
                                                                                       .Tuning(r => r.RepeatCount, null)
                                                                                       .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2015 16:42")))
                                                                             .ShouldBeIsResult(Convert.ToDateTime("05/15/2015 21:36"));

        It should_be_get_recurrency_date_hourly_with_repeat_count = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Hourly)
                                                                                            .Tuning(r => r.RepeatInterval, 5)
                                                                                            .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 00:36"))
                                                                                            .Tuning(r => r.EndDate, null)
                                                                                            .Tuning(r => r.RepeatCount, 3)
                                                                                            .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2015 14:38")))
                                                                                  .ShouldBeIsResult(Convert.ToDateTime("05/15/2015 15:36"));

        It should_be_get_recurrency_date_minutely_with_end_date = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Minutely)
                                                                                          .Tuning(r => r.RepeatInterval, 5)
                                                                                          .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                          .Tuning(r => r.EndDate, Convert.ToDateTime("05/15/2015 20:23"))
                                                                                          .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2015 16:37")))
                                                                                .ShouldBeIsResult(Convert.ToDateTime("05/15/2015 16:41"));

        It should_be_get_recurrency_date_minutely_with_endless = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Minutely)
                                                                                         .Tuning(r => r.RepeatInterval, 5)
                                                                                         .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                         .Tuning(r => r.EndDate, null)
                                                                                         .Tuning(r => r.RepeatCount, null)
                                                                                         .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2015 16:42")))
                                                                               .ShouldBeIsResult(Convert.ToDateTime("05/15/2015 16:46"));

        It should_be_get_recurrency_date_minutely_with_repeat_count = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Minutely)
                                                                                              .Tuning(r => r.RepeatInterval, 5)
                                                                                              .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                              .Tuning(r => r.EndDate, null)
                                                                                              .Tuning(r => r.RepeatCount, 3)
                                                                                              .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2015 16:38")))
                                                                                    .ShouldBeIsResult(Convert.ToDateTime("05/15/2015 16:41"));

        It should_be_get_recurrency_date_monthly_with_end_date = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Monthly)
                                                                                         .Tuning(r => r.RepeatInterval, 1)
                                                                                         .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                         .Tuning(r => r.EndDate, Convert.ToDateTime("08/20/2015 20:23"))
                                                                                         .Tuning(r => r.NowDate, Convert.ToDateTime("07/19/2015 16:37")))
                                                                               .ShouldBeIsResult(Convert.ToDateTime("08/15/2015 16:36"));

        It should_be_get_recurrency_date_monthly_with_endless = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Monthly)
                                                                                        .Tuning(r => r.RepeatInterval, 1)
                                                                                        .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                        .Tuning(r => r.EndDate, null)
                                                                                        .Tuning(r => r.RepeatCount, null)
                                                                                        .Tuning(r => r.NowDate, Convert.ToDateTime("08/15/2015 16:42")))
                                                                              .ShouldBeIsResult(Convert.ToDateTime("09/15/2015 16:36"));

        It should_be_get_recurrency_date_monthly_with_repeat_count = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Monthly)
                                                                                             .Tuning(r => r.RepeatInterval, 1)
                                                                                             .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                             .Tuning(r => r.EndDate, null)
                                                                                             .Tuning(r => r.RepeatCount, 3)
                                                                                             .Tuning(r => r.NowDate, Convert.ToDateTime("07/19/2015 16:38")))
                                                                                   .ShouldBeIsResult(Convert.ToDateTime("08/15/2015 16:36"));

        It should_be_get_recurrency_date_weekly_with_end_date = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                        .Tuning(r => r.RepeatInterval, 2)
                                                                                        .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                        .Tuning(r => r.EndDate, Convert.ToDateTime("06/16/2015 23:00"))
                                                                                        .Tuning(r => r.RepeatCount, null)
                                                                                        .Tuning(r => r.RepeatDays, null)
                                                                                        .Tuning(r => r.NowDate, Convert.ToDateTime("05/30/2015 16:00")))
                                                                              .ShouldBeIsResult(Convert.ToDateTime("06/12/2015 16:00"));

        It should_be_get_recurrency_date_weekly_with_end_date_and_days_first = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                                       .Tuning(r => r.RepeatInterval, 2)
                                                                                                       .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                       .Tuning(r => r.EndDate, Convert.ToDateTime("06/16/2015 23:00"))
                                                                                                       .Tuning(r => r.RepeatCount, null)
                                                                                                       .Tuning(r => r.RepeatDays, GetRecurrencyDateQuery.DayOfWeek.Monday | GetRecurrencyDateQuery.DayOfWeek.Sunday)
                                                                                                       .Tuning(r => r.NowDate, Convert.ToDateTime("05/30/2015 16:00")))
                                                                                             .ShouldBeIsResult(Convert.ToDateTime("05/31/2015 16:00"));

        It should_be_get_recurrency_date_weekly_with_end_date_and_days_second = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                                        .Tuning(r => r.RepeatInterval, 2)
                                                                                                        .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                        .Tuning(r => r.EndDate, Convert.ToDateTime("06/16/2015 23:00"))
                                                                                                        .Tuning(r => r.RepeatCount, null)
                                                                                                        .Tuning(r => r.RepeatDays, GetRecurrencyDateQuery.DayOfWeek.Monday | GetRecurrencyDateQuery.DayOfWeek.Friday)
                                                                                                        .Tuning(r => r.NowDate, Convert.ToDateTime("05/30/2015 16:00")))
                                                                                              .ShouldBeIsResult(Convert.ToDateTime("06/08/2015 16:00"));

        It should_be_get_recurrency_date_weekly_with_endless = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                       .Tuning(r => r.RepeatInterval, 2)
                                                                                       .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                       .Tuning(r => r.EndDate, null)
                                                                                       .Tuning(r => r.RepeatCount, null)
                                                                                       .Tuning(r => r.RepeatDays, null)
                                                                                       .Tuning(r => r.NowDate, Convert.ToDateTime("05/30/2015 16:00")))
                                                                             .ShouldBeIsResult(Convert.ToDateTime("06/12/2015 16:00"));

        It should_be_get_recurrency_date_weekly_with_endless_and_days_first = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                                      .Tuning(r => r.RepeatInterval, 2)
                                                                                                      .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                      .Tuning(r => r.EndDate, null)
                                                                                                      .Tuning(r => r.RepeatCount, null)
                                                                                                      .Tuning(r => r.RepeatDays, GetRecurrencyDateQuery.DayOfWeek.Monday | GetRecurrencyDateQuery.DayOfWeek.Sunday)
                                                                                                      .Tuning(r => r.NowDate, Convert.ToDateTime("05/30/2015 15:00")))
                                                                                            .ShouldBeIsResult(Convert.ToDateTime("05/31/2015 16:00"));

        It should_be_get_recurrency_date_weekly_with_endless_and_days_second = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                                       .Tuning(r => r.RepeatInterval, 2)
                                                                                                       .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                       .Tuning(r => r.EndDate, null)
                                                                                                       .Tuning(r => r.RepeatCount, null)
                                                                                                       .Tuning(r => r.RepeatDays, GetRecurrencyDateQuery.DayOfWeek.Monday | GetRecurrencyDateQuery.DayOfWeek.Friday)
                                                                                                       .Tuning(r => r.NowDate, Convert.ToDateTime("05/30/2015 15:00")))
                                                                                             .ShouldBeIsResult(Convert.ToDateTime("06/08/2015 16:00"));

        It should_be_get_recurrency_date_weekly_with_repeat_count = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                            .Tuning(r => r.RepeatInterval, 2)
                                                                                            .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                            .Tuning(r => r.EndDate, null)
                                                                                            .Tuning(r => r.RepeatCount, 2)
                                                                                            .Tuning(r => r.RepeatDays, null)
                                                                                            .Tuning(r => r.NowDate, Convert.ToDateTime("05/30/2015 16:00")))
                                                                                  .ShouldBeIsResult(Convert.ToDateTime("06/12/2015 16:00"));

        It should_be_get_recurrency_date_weekly_with_repeat_count_and_days_first = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                                           .Tuning(r => r.RepeatInterval, 2)
                                                                                                           .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                           .Tuning(r => r.EndDate, null)
                                                                                                           .Tuning(r => r.RepeatCount, 2)
                                                                                                           .Tuning(r => r.RepeatDays, GetRecurrencyDateQuery.DayOfWeek.Monday | GetRecurrencyDateQuery.DayOfWeek.Sunday)
                                                                                                           .Tuning(r => r.NowDate, Convert.ToDateTime("05/30/2015 16:00")))
                                                                                                 .ShouldBeIsResult(Convert.ToDateTime("05/31/2015 16:00"));

        It should_be_get_recurrency_date_weekly_with_repeat_count_and_days_second = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Weekly)
                                                                                                            .Tuning(r => r.RepeatInterval, 2)
                                                                                                            .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:00"))
                                                                                                            .Tuning(r => r.EndDate, null)
                                                                                                            .Tuning(r => r.RepeatCount, 2)
                                                                                                            .Tuning(r => r.RepeatDays, GetRecurrencyDateQuery.DayOfWeek.Monday | GetRecurrencyDateQuery.DayOfWeek.Tuesday)
                                                                                                            .Tuning(r => r.NowDate, Convert.ToDateTime("05/30/2015 16:00")))
                                                                                                  .ShouldBeIsResult(Convert.ToDateTime("06/08/2015 16:00"));

        It should_be_get_recurrency_date_yearly_with_end_date = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Yearly)
                                                                                        .Tuning(r => r.RepeatInterval, 5)
                                                                                        .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                        .Tuning(r => r.EndDate, Convert.ToDateTime("05/15/2021 20:23"))
                                                                                        .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2020 16:33")))
                                                                              .ShouldBeIsResult(Convert.ToDateTime("05/15/2020 16:36"));

        It should_be_get_recurrency_date_yearly_with_endless = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Yearly)
                                                                                       .Tuning(r => r.RepeatInterval, 5)
                                                                                       .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                       .Tuning(r => r.EndDate, null)
                                                                                       .Tuning(r => r.RepeatCount, null)
                                                                                       .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2015 16:42")))
                                                                             .ShouldBeIsResult(Convert.ToDateTime("05/15/2020 16:36"));

        It should_be_get_recurrency_date_yearly_with_repeat_count = () => Execute(dsl => dsl.Tuning(r => r.Type, GetRecurrencyDateQuery.RepeatType.Yearly)
                                                                                            .Tuning(r => r.RepeatInterval, 5)
                                                                                            .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                            .Tuning(r => r.EndDate, null)
                                                                                            .Tuning(r => r.RepeatCount, 3)
                                                                                            .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2020 16:30")))
                                                                                  .ShouldBeIsResult(Convert.ToDateTime("05/15/2020 16:36"));
    }
}