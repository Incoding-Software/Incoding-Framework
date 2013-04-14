namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_query_with_paginated_specification : Context_nhibernate_repository_query
    {
        #region Estabilish value

        static IList<RealDbEntity> result;

        #endregion

        Because of = () => { result = repository.Query<RealDbEntity>(paginatedSpecification: new PaginatedSpecification(1, 5)).ToList(); };

        It should_be_count = () => result.Count().ShouldEqual(5);
    }
}