namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using NHibernate.Linq;

    #endregion

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_paginated_all_where_specification : Context_nhibernate_repository_query
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

        #endregion

        #region Estabilish value

        static string[] ids;

        static IncPaginatedResult<RealDbEntity> result;

        #endregion

        Because of = () =>
                         {
                             ids = Session.Query<RealDbEntity>()
                                          .Take(7)
                                          .ToList().Select(r => r.Id.ToString())
                                          .ToArray();

                             result = repository.Paginated(new PaginatedSpecification(1, 5), 
                                                           orderSpecification: new FakeOrder(), 
                                                           fetchSpecification: new FakeFetch(), 
                                                           whereSpecification: new EntityContainIdSpec<RealDbEntity>(ids));
                         };

        It should_be_total_count = () => result.TotalCount.ShouldEqual(7);

        It should_be_page = () => result.Items.Count.ShouldEqual(5);

        It should_be_fetch = () =>
                                 {
                                     Session.Close();
                                     foreach (var item in result.Items)
                                         item.Items[0].Name.ShouldNotBeEmpty();
                                 };
    }
}