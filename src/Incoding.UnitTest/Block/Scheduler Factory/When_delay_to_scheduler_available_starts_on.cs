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

    [Subject(typeof(DelayToSchedulerAvaialbeStartsOnWhereSpec))]
    public class When_delay_to_scheduler_available_starts_on
    {
        #region Establish value

        static IQueryable<DelayToScheduler> fakeCollection;

        static List<DelayToScheduler> filterCollection;

        static DateTime oneMinuteInFeature;

        static DateTime twoMinutesInFeature;

        static DateTime oneMinutesAgo;

        static DateTime twoMinutesAgo;

        static DateTime currentDate;

        #endregion

        Establish establish = () =>
                                  {
                                      Func<DateTime, DelayToScheduler> createEntity = (startsOn) => Pleasure.MockStrictAsObject<DelayToScheduler>(mock => mock.SetupGet(r => r.StartsOn).Returns(startsOn));

                                      currentDate = DateTime.Now;
                                      oneMinuteInFeature = currentDate.AddMinutes(1);
                                      twoMinutesInFeature = currentDate.AddMinutes(2);
                                      oneMinutesAgo = currentDate.AddMinutes(-1);
                                      twoMinutesAgo = currentDate.AddMinutes(-2);
                                      fakeCollection = Pleasure.ToQueryable(createEntity(currentDate.AddMinutes(-5)),
                                                                            createEntity(oneMinuteInFeature),
                                                                            createEntity(twoMinutesInFeature),
                                                                            createEntity(oneMinutesAgo),
                                                                            createEntity(twoMinutesInFeature),
                                                                            createEntity(currentDate.AddMinutes(10)));
                                  };

        Because of = () =>
                         {
                             filterCollection = fakeCollection
                                     .Where(new DelayToSchedulerAvaialbeStartsOnWhereSpec(currentDate).IsSatisfiedBy())
                                     .ToList();
                         };

        It should_be_filter = () =>
                                  {
                                      filterCollection.Count.ShouldEqual(4);
                                      filterCollection.Select(r => r.StartsOn)
                                                      .ShouldEqualWeakEach(new[]
                                                                               {
                                                                                       oneMinuteInFeature, twoMinutesInFeature,
                                                                                       oneMinutesAgo, twoMinutesAgo
                                                                               });
                                  };
    }
}