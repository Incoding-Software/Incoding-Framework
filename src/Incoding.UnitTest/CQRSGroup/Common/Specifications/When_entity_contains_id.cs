namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(EntityContainIdSpec<IncEntityBase>))]
    public class When_entity_contains_id
    {
        #region Estabilish value

        static IQueryable<IncEntityBase> fakeCollection;

        static List<IncEntityBase> filterCollection;

        static string[] containsId;

        #endregion

        Establish establish = () =>
                                  {
                                      Func<string, IncEntityBase> createEntity = (id) => Pleasure.MockStrictAsObject<IncEntityBase>(mock => mock.SetupGet(r => r.Id).Returns(id));
                                      containsId = new[] { Pleasure.Generator.String(), Pleasure.Generator.String() };
                                      fakeCollection = Pleasure.ToQueryable(createEntity(containsId[0]), createEntity(containsId[1]));
                                  };

        Because of = () =>
                         {
                             filterCollection = fakeCollection
                                     .Where(new EntityContainIdSpec<IncEntityBase>(containsId).IsSatisfiedBy())
                                     .ToList();
                         };

        It should_be_filter = () =>
                                  {
                                      filterCollection.Count.ShouldEqual(2);
                                      filterCollection[0].Id.ShouldEqual(containsId[0]);
                                      filterCollection[1].Id.ShouldEqual(containsId[1]);
                                  };
    }
}