namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Linq;
    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_query : Context_nhibernate_repository_query
    {
        #region Estabilish value

        static IQueryable<RealDbEntity> result;

        #endregion

        Because of = () => { result = repository.Query<RealDbEntity>(); };

        It should_be_count = () => result.Count().ShouldEqual(10);
    }
}