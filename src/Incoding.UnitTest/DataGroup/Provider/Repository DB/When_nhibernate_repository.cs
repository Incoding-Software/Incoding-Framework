namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;
    using NCrunch.Framework;

    #endregion

    [Subject(typeof(NhibernateRepository)), Isolated]
    public class When_nhibernate_repository : Behavior_repository
    {
        #region Establish value

        protected static IRepository GetRepository()
        {
            var openSession = MSpecAssemblyContext.NhibernateFluent().BuildSessionFactory().OpenSession();
            return new NhibernateRepository(openSession);
        }

        #endregion

        Because of = () =>
                     {
                         repository = GetRepository();
                         repository.Init();
                     };

        Behaves_like<Behavior_repository> should_be_behavior;
    }
}