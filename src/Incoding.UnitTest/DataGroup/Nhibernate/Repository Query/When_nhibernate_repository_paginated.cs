namespace Incoding.UnitTest
{
    using Incoding.Data;
    using Machine.Specifications;

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_paginated : Context_nhibernate_repository_query
    {
        #region Estabilish value

        static IncPaginatedResult<RealDbEntity> result;

        #endregion

        Because of = () => { result = repository.Paginated<RealDbEntity>(new PaginatedSpecification(1, 5)); };

        It should_be_total_count = () => result.TotalCount.ShouldEqual(10);

        It should_be_result = () => result.Items.Count.ShouldEqual(5);
    }
}