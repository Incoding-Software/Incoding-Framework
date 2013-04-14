namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using NHibernate;
    using NHibernate.Context;

    #endregion

    [Subject(typeof(NhibernateSessionFactory))]
    public class When_nhibernate_session_factory_open_session_with_connection_string
    {
        #region Estabilish value

        static NhibernateSessionFactory sessionFactory;

        static ISession currentSession;

        static SqlConnection connection;

        #endregion

        Establish establish = () =>
                                  {
                                      var fluentConfiguration = Fluently
                                              .Configure()
                                              .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.ConnectionStrings["IncRealDb"].ConnectionString))
                                              .CurrentSessionContext<ThreadStaticSessionContext>();

                                      connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IncRealDb"].ConnectionString);
                                      sessionFactory = new NhibernateSessionFactory(fluentConfiguration);
                                  };

        Because of = () => { currentSession = sessionFactory.OpenSession(connection); };

        It should_be_same_session = () => currentSession.ShouldNotBeNull();

        It should_be_open_connection = () => connection.State.ShouldEqual(ConnectionState.Open);
    }
}