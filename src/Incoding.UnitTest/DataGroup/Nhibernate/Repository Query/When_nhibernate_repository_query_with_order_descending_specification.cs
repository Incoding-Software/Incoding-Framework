namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_query_with_order_descending_specification : Context_nhibernate_repository_query
    {
        #region Fake classes

        class FakeOrderSpecification : OrderSpecification<RealDbEntity>
        {
            public override Action<AdHocOrderSpecification<RealDbEntity>> SortedBy()
            {
                return specification => specification
                                                .OrderByDescending(r => r.Value)
                                                .OrderBy(r => r.Value2);
            }
        }

        #endregion

        #region Estabilish value

        static List<RealDbEntity> result;

        #endregion

        Because of = () => { result = repository.Query(orderSpecification: new FakeOrderSpecification()).ToList(); };

        It should_be_sorted = () =>
                                  {
                                      var sortedCollection = result.Take(3).ToList();

                                      sortedCollection[0].Value.ShouldBeGreaterThan(sortedCollection[1].Value);
                                      sortedCollection[1].Value.ShouldBeGreaterThan(sortedCollection[2].Value);
                                  };
    }
}