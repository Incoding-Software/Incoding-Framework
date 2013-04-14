namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(EntityByIdSpec<>))]
    public class When_entity_by_id
    {
        #region Estabilish value

        static IQueryable<IncEntityBase> fakeCollection;

        static List<IncEntityBase> filterCollection;

        #endregion

        Establish establish = () =>
                                  {
                                      Func<string, IncEntityBase> createEntity = id => Pleasure.MockStrictAsObject<IncEntityBase>(mock => mock.SetupGet(r => r.Id).Returns(id));

                                      fakeCollection = Pleasure.ToQueryable(createEntity(Pleasure.Generator.TheSameString()), createEntity(Pleasure.Generator.String()));
                                  };

        Because of = () =>
                         {
                             filterCollection = fakeCollection
                                     .Where(new EntityByIdSpec<IncEntityBase>(Pleasure.Generator.TheSameString()).IsSatisfiedBy())
                                     .ToList();
                         };

        It should_be_filter = () =>
                                  {
                                      filterCollection.Count.ShouldEqual(1);
                                      filterCollection[0].Should(user => user.Id.ShouldEqual(Pleasure.Generator.TheSameString()));
                                  };
    }
}