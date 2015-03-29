namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.Block;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DelayToScheduler.Reccurence))]
    public class When_reccurrence
    {
        #region Establish value

        static DateTime dt = Pleasure.Generator.Invent<DateTime>();

        #endregion

        It should_be_none = () =>
                                {
                                    var original = Pleasure.Generator.Invent<DelayToScheduler.Reccurence>(dsl => dsl.Tuning(r => r.Repeats, DelayToScheduler.RepeatOfType.None)
                                                                                                                    .Tuning(r => r.EndsOfAfter, null)
                                                                                                                    .Tuning(r => r.EndsOfDt, null)
                                                                                                                    .Tuning(r => r.StartsOn, dt));
                                    original.NextDt().ShouldEqual(dt);
                                };

        It should_be_daily_with_repeat_never = () =>
                                                   {
                                                       var original = Pleasure.Generator.Invent<DelayToScheduler.Reccurence>(dsl => dsl.Tuning(r => r.Repeats, DelayToScheduler.RepeatOfType.Daily)
                                                                                                                                       .Tuning(r => r.StartsOn, dt)
                                                                                                                                       .Tuning(r => r.EndsOfAfter, null)
                                                                                                                                       .Tuning(r => r.EndsOfDt, null)
                                                                                                                                       .Tuning(r => r.RepeatEvery, 2));

                                                       for (int i = 1; i < Pleasure.Generator.PositiveNumber(2, 20); i++)
                                                           original.NextDt().ShouldEqual(dt.AddDays(i * 2));
                                                   };

        It should_be_daily_with_repeat_ends_of_dt = () =>
                                                        {
                                                            var original = Pleasure.Generator.Invent<DelayToScheduler.Reccurence>(dsl => dsl.Tuning(r => r.Repeats, DelayToScheduler.RepeatOfType.Daily)
                                                                                                                                            .Tuning(r => r.StartsOn, dt)
                                                                                                                                            .Tuning(r => r.EndsOfDt, dt.AddDays(4))
                                                                                                                                            .Tuning(r => r.EndsOfAfter, null)
                                                                                                                                            .Tuning(r => r.RepeatEvery, 2));

                                                            original.NextDt().ShouldEqual(dt.AddDays(2));
                                                            original.NextDt().ShouldEqual(dt.AddDays(4));
                                                            original.NextDt().ShouldBeNull();
                                                        };

        It should_be_daily_with_repeat_ends_of_after = () =>
                                                           {
                                                               var original = Pleasure.Generator.Invent<DelayToScheduler.Reccurence>(dsl => dsl.Tuning(r => r.Repeats, DelayToScheduler.RepeatOfType.Daily)
                                                                                                                                               .Tuning(r => r.StartsOn, dt)
                                                                                                                                               .Tuning(r => r.EndsOfDt, null)
                                                                                                                                               .Tuning(r => r.EndsOfAfter, 3)
                                                                                                                                               .Tuning(r => r.RepeatEvery, 2));

                                                               original.NextDt().ShouldEqual(dt.AddDays(2));
                                                               original.NextDt().ShouldEqual(dt.AddDays(4));
                                                               original.NextDt().ShouldEqual(dt.AddDays(6));
                                                               original.NextDt().ShouldBeNull();
                                                           };    
        
        It should_be_weekly_with_repeat_never = () =>
                                                           {
                                                               var original = Pleasure.Generator.Invent<DelayToScheduler.Reccurence>(dsl => dsl.Tuning(r => r.Repeats, DelayToScheduler.RepeatOfType.Weekly)
                                                                                                                                      .Tuning(r => r.StartsOn, dt)
                                                                                                                                      .Tuning(r => r.EndsOfAfter, null)
                                                                                                                                      .Tuning(r => r.EndsOfDt, null)
                                                                                                                                      .Tuning(r => r.RepeatEvery, 2)
                                                                                                                                      .Tuning(r=>r.RepeatOn,DelayToScheduler.DayOfWeek.Friday | DelayToScheduler.DayOfWeek.Sunday));

                                                               for (int i = 1; i < Pleasure.Generator.PositiveNumber(2, 20); i++)
                                                                   original.NextDt().ShouldEqual(dt.AddDays(i * 2));
                                                           };
    }
}