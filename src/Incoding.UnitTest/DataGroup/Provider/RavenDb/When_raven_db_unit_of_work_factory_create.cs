namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Raven.Client;

    #endregion

    [Subject(typeof(RavenDbUnitOfWorkFactory))]
    public class When_raven_db_unit_of_work_factory_create : Behavior_unit_of_work_factory
    {
        Because of = () =>
                     {
                         var document = Pleasure.MockAsObject<IDocumentSession>();
                         var sessionFactory = Pleasure.MockAsObject<IRavenDbSessionFactory>(mock => mock.Setup(r => r.Open(connectionString)).Returns(document));
                         unitOfWork = new RavenDbUnitOfWorkFactory(sessionFactory)
                                 .Create(isolated, isFlush, connectionString);
                     };

        Behaves_like<Behavior_unit_of_work_factory> verify;
    }
}