namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncEntityBase))]
    public class When_entities_distinct : Context_entity_base
    {
        #region Estabilish value

        static List<IncEntityBase> entityCollection;

        static IEnumerable<IncEntityBase> distinctCollection;

        #endregion

        Establish establish = () =>
                                  {
                                      var sameId = Guid.NewGuid();
                                      entityCollection = new List<IncEntityBase>
                                                             {
                                                                     new FakeEntityBase(sameId), 
                                                                     new FakeEntityBase(sameId), 
                                                                     new FakeEntityBase(sameId), 
                                                                     new FakeEntityBase(sameId), 
                                                                     new FakeEntityBase(Guid.NewGuid())
                                                             };
                                  };

        Because of = () => { distinctCollection = entityCollection.Distinct(); };

        It should_be_stay_only_unique_entity = () => distinctCollection.Count().ShouldEqual(2);
    }
}