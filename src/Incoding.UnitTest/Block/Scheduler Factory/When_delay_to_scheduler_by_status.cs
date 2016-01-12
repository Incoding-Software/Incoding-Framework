namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DelayToScheduler.Where.ByStatus))]
    public class When_delay_to_scheduler_by_status
    {
        #region Establish value

        static IQueryable<DelayToScheduler> fakeCollection;

        static List<DelayToScheduler> filterCollection;

        #endregion

        Establish establish = () =>
                                  {
                                      Func<DelayOfStatus, DelayToScheduler> createEntity = (status) => Pleasure.MockAsObject<DelayToScheduler>(mock => mock.SetupGet(r => r.Status).Returns(status));

                                      fakeCollection = Pleasure.ToQueryable(createEntity(DelayOfStatus.New), 
                                                                            createEntity(DelayOfStatus.New.Inverse()), 
                                                                            createEntity(DelayOfStatus.New.Inverse()));
                                  };

        Because of = () =>
                         {
                             filterCollection = fakeCollection
                                     .Where(new DelayToScheduler.Where.ByStatus(DelayOfStatus.New).IsSatisfiedBy())
                                     .ToList();
                         };

        It should_be_filter = () =>
                                  {
                                      filterCollection.Count.ShouldEqual(1);
                                      filterCollection[0].Status.ShouldEqual(DelayOfStatus.New);
                                  };
    }
}