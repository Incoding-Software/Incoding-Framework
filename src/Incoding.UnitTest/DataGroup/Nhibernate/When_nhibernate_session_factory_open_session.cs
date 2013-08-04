namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Configuration;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Incoding.Data;
    using Machine.Specifications;
    using NHibernate;
    using NHibernate.Context;

    #endregion

    [Subject(typeof(NhibernateSessionFactory))]
    public class When_nhibernate_session_factory_open_session
    {
        #region Estabilish value

        static NhibernateSessionFactory sessionFactory;

        static ISession currentSession;

        #endregion

        Establish establish = () =>
                                  {
                                      var fluentConfiguration = Fluently
                                              .Configure()
                                              .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.ConnectionStrings["IncRealDb"].ConnectionString))
                                              .CurrentSessionContext<ThreadStaticSessionContext>();

                                      sessionFactory = new NhibernateSessionFactory(fluentConfiguration);
                                  };

        Because of = () => { currentSession = sessionFactory.OpenSession(null); };

        It should_be_same_session = () => currentSession.ShouldNotBeNull();
    }
}