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

    [Subject(typeof(DelayToSchedulerByUIDWhereSpec))]
    public class When_delay_to_scheduler_by_uid
    {
        #region Establish value

        static IQueryable<DelayToScheduler> fakeCollection;

        static List<DelayToScheduler> filterCollection;

        #endregion

        Establish establish = () =>
                                  {
                                      Func<string, DelayToScheduler> createEntity = (uid) => Pleasure.MockAsObject<DelayToScheduler>(mock => mock.SetupGet(r => r.UID).Returns(uid));

                                      fakeCollection = Pleasure.ToQueryable(createEntity(Pleasure.Generator.TheSameString()), 
                                                                            createEntity(Pleasure.Generator.String()));
                                  };

        Because of = () =>
                         {
                             filterCollection = fakeCollection
                                     .Where(new DelayToSchedulerByUIDWhereSpec(Pleasure.Generator.TheSameString()).IsSatisfiedBy())
                                     .ToList();
                         };

        It should_be_filter = () =>
                                  {
                                      filterCollection.Count.ShouldEqual(1);
                                      filterCollection[0].UID.ShouldBeTheSameString();
                                  };
    }
}