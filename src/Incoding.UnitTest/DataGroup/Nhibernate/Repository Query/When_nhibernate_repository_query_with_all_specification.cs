namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(NhibernateRepository)), Ignore("Nhibernate broken")]
    public class When_nhibernate_repository_query_with_all_specification : Context_nhibernate_repository_query
    {
        #region Fake classes

        class FakeFetch : FetchSpecification<RealDbEntity>
        {
            public override Action<AdHocFetchSpecification<RealDbEntity>> FetchedBy()
            {
                return specification => specification.Join(r => r.Items);
            }
        }

        class FakeOrder : OrderSpecification<RealDbEntity>
        {
            public override Action<AdHocOrderSpecification<RealDbEntity>> SortedBy()
            {
                return specification => specification.OrderBy(r => r.Value);
            }
        }

        class FakeWhere : Specification<RealDbEntity>
        {
            public override Expression<Func<RealDbEntity, bool>> IsSatisfiedBy()
            {
                return entity => true;
            }
        }

        #endregion

        #region Estabilish value

        static List<RealDbEntity> result;

        #endregion

        Because of = () =>
                         {
                             result = repository.Query(paginatedSpecification: new PaginatedSpecification(1, 5), 
                                                       orderSpecification: new FakeOrder(), 
                                                       whereSpecification: new FakeWhere(), 
                                                       fetchSpecification: new FakeFetch())
                                                .ToList();
                         };

        It should_be_count = () => result.Count.ShouldEqual(5);

        It should_be_fetch = () =>
                                 {
                                     Session.Close();
                                     foreach (var realDbEntity in result)
                                         realDbEntity.Items[0].Name.ShouldNotBeEmpty();
                                 };
    }
}