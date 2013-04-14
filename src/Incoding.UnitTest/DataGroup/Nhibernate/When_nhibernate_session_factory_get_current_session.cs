namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Configuration;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using NHibernate;
    using NHibernate.Context;

    #endregion

    [Subject(typeof(NhibernateSessionFactory))]
    public class When_nhibernate_session_factory_get_current_session
    {
        #region Estabilish value

        static NhibernateSessionFactory sessionFactory;

        static ISession currentSession;

        static ISession session;

        #endregion

        Establish establish = () =>
                                  {
                                      var fluentConfiguration = Fluently
                                              .Configure()
                                              .Database(MsSqlConfiguration.MsSql2008
                                                                          .ConnectionString(ConfigurationManager.ConnectionStrings["IncRealDb"].ConnectionString))
                                              .CurrentSessionContext<ThreadStaticSessionContext>();

                                      sessionFactory = new NhibernateSessionFactory(fluentConfiguration);
                                      session = fluentConfiguration.BuildSessionFactory().OpenSession();
                                      CurrentSessionContext.Bind(session);
                                  };

        Because of = () => { currentSession = sessionFactory.GetCurrentSession(); };

        It should_be_same_session = () => currentSession.ShouldBeTheSameAs(session);
    }
}