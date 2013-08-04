namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using System.Data.SqlClient;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using NHibernate;
    using NHibernate.Context;
    using NHibernate.Engine;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(NhibernateUnitOfWorkFactory))]
    public class When_nhibernate_unit_of_work_factory_create
    {
        #region Estabilish value

        static Mock<INhibernateSessionFactory> sessionFactory;

        static NhibernateUnitOfWorkFactory unitOfWorkFactory;

        static IUnitOfWork unitOfWork;

        static SqlConnection connectionString;

        #endregion

        Establish establish = () =>
                                  {
                                      var session = Pleasure.Mock<ISession>(mock => mock.Setup(r => r.SessionFactory)
                                                                                        .Returns(Pleasure.MockAsObject<ISessionFactoryImplementor>(sessionFactoryMock => sessionFactoryMock.SetupGet(r => r.CurrentSessionContext)
                                                                                                                                                                                           .Returns(Pleasure.MockAsObject<CurrentSessionContext>()))));

                                      connectionString = new SqlConnection(@"Data Source=Work\SQLEXPRESS;Database=IncRealDb;Integrated Security=true;");
                                      sessionFactory = Pleasure.Mock<INhibernateSessionFactory>(mock => mock.Setup(r => r.OpenSession(Pleasure.MockIt.IsNotNull<SqlConnection>()))
                                                                                                            .Returns(session.Object));
                                      unitOfWorkFactory = new NhibernateUnitOfWorkFactory(sessionFactory.Object);
                                  };

        Because of = () => { unitOfWork = unitOfWorkFactory.Create(IsolationLevel.ReadCommitted, connectionString); };

        It should_be_transaction_active = () => unitOfWork.ShouldNotBeNull();

        It should_be_open_session = () => sessionFactory.Verify(r => r.OpenSession(Pleasure.MockIt.IsNotNull<SqlConnection>()));
    }
}