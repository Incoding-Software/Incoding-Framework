namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Linq;
    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_query_with_order_specification : Context_nhibernate_repository_query
    {
        #region Fake classes

        class FakeOrderSpecification : OrderSpecification<RealDbEntity>
        {
            public override Action<AdHocOrderSpecification<RealDbEntity>> SortedBy()
            {
                return specification => specification
                                                .OrderBy(r => r.Value)
                                                .OrderByDescending(r => r.Value2);
            }
        }

        #endregion

        #region Estabilish value

        static IQueryable<RealDbEntity> result;

        #endregion

        Because of = () => { result = repository.Query(orderSpecification: new FakeOrderSpecification()); };

        It should_be_sorted = () =>
                                  {
                                      var sortedCollection = result.ToList();
                                      sortedCollection[0].Value.ShouldBeLessThan(sortedCollection[1].Value);
                                      sortedCollection[1].Value.ShouldBeLessThan(sortedCollection[2].Value);
                                      sortedCollection[2].Value.ShouldBeLessThan(sortedCollection[3].Value);
                                  };
    }
}