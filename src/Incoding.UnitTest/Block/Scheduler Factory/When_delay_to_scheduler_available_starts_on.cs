namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Block;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DelayToScheduler.Where.AvailableStartsOn))]
    public class When_delay_to_scheduler_available_starts_on
    {
        Establish establish = () =>
                              {
                                  Func<DateTime, DelayToScheduler> createEntity = (startsOn) => Pleasure.MockAsObject<DelayToScheduler>(mock => { mock.SetupGet(r => r.StartsOn).Returns(startsOn); });

                                  currentDate = DateTime.Now;
                                  oneMinuteInFeature = currentDate.AddMinutes(1);
                                  twoMinutesInFeature = currentDate.AddMinutes(2);
                                  var threeOrMoreMinuteInFeature = currentDate.AddMinutes(Pleasure.Generator.PositiveNumber(minValue: 3, maxValue: 100));
                                  anyMinuteAgo = currentDate.AddMinutes(-Pleasure.Generator.PositiveNumber(maxValue: 50));
                                  nextAnyMinuteAgo = currentDate.AddMinutes(-Pleasure.Generator.PositiveNumber(minValue: 51, maxValue: 100));
                                  fakeCollection = Pleasure.ToQueryable(createEntity(oneMinuteInFeature),
                                                                        createEntity(twoMinutesInFeature),
                                                                        createEntity(anyMinuteAgo),                                                                        
                                                                        createEntity(nextAnyMinuteAgo),                                                                        
                                                                        createEntity(threeOrMoreMinuteInFeature));
                              };

        Because of = () =>
                     {
                         filterCollection = fakeCollection
                                 .Where(new DelayToScheduler.Where.AvailableStartsOn(currentDate).IsSatisfiedBy())
                                 .ToList();
                     };

        It should_be_filter = () =>
                              {
                                  filterCollection.Select(r => r.StartsOn)
                                                  .OrderByDescending(r => r)
                                                  .ShouldEqualWeakEach(new[]
                                                                       {
                                                                               twoMinutesInFeature,
                                                                               oneMinuteInFeature,
                                                                               anyMinuteAgo,
                                                                               nextAnyMinuteAgo,
                                                                       });
                              };

        #region Establish value

        static IQueryable<DelayToScheduler> fakeCollection;

        static List<DelayToScheduler> filterCollection;

        static DateTime oneMinuteInFeature;

        static DateTime twoMinutesInFeature;

        static DateTime anyMinuteAgo;

        static DateTime nextAnyMinuteAgo;

        static DateTime currentDate;

        #endregion
    }
}