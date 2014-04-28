namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using System.Data.SqlClient;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(RavenDbUnitOfWorkFactory))]
    public class When_raven_db_unit_of_work_factory_create
    {
        #region Establish value

        static RavenDbUnitOfWorkFactory factory;

        static IUnitOfWork unitOfWork;

        #endregion

        Establish establish = () =>
                                  {
                                      var sessionFactory = Pleasure.MockStrictAsObject<IRavenDbSessionFactory>();
                                      factory = new RavenDbUnitOfWorkFactory(sessionFactory);
                                  };

        Because of = () => { unitOfWork = factory.Create(IsolationLevel.ReadCommitted, Pleasure.Generator.String()); };

        It should_be_create = () => unitOfWork.ShouldNotBeNull();
    }
}