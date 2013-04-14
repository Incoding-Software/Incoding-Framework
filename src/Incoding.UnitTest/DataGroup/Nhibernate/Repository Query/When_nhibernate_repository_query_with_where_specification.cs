namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using NHibernate.Linq;

    #endregion

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_query_with_where_specification : Context_nhibernate_repository_query
    {
        #region Estabilish value

        static IQueryable<RealDbEntity> result;

        static object idEntity;

        #endregion

        Because of = () =>
                         {
                             idEntity = Session.Query<RealDbEntity>().First().Id;
                             result = repository.Query(whereSpecification: new EntityByIdSpec<RealDbEntity>(idEntity.ToString()));
                         };

        It should_be_count = () => result.Count(r => r.Id == idEntity).ShouldEqual(1);
    }
}