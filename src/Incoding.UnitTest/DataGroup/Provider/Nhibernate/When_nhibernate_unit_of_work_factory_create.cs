namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using NHibernate;

    #endregion

    [Subject(typeof(NhibernateUnitOfWorkFactory))]
    public class When_nhibernate_unit_of_work_factory_create : Behavior_unit_of_work_factory
    {
        Establish establish = () =>
                              {
                                  var session = Pleasure.MockAsObject<ISession>();
                                  var sessionFactory = Pleasure.MockStrictAsObject<INhibernateSessionFactory>(mock => mock.Setup(r => r.Open(connectionString)).Returns(session));
                                  unitOfWork = new NhibernateUnitOfWorkFactory(sessionFactory).Create(isolated, isFlush, connectionString);
                              };

        Behaves_like<Behavior_unit_of_work_factory> verify;
    }
}