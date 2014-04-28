namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Reflection;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using NCrunch.Framework;
    using NHibernate;

    #endregion

    [Subject(typeof(NhibernateSessionFactory)), Isolated]
    public class When_nhibernate_session_factory_get_current_session
    {
        #region Establish value

        static ISession session;

        static NhibernateSessionFactory sessionFactory;

        static ISession currentSession;

        #endregion

        Establish establish = () =>
                                  {
                                      session = Pleasure.MockStrictAsObject<ISession>();
                                      typeof(NhibernateSessionFactory).GetField("currentSession", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, session);
                                      sessionFactory = new NhibernateSessionFactory(Pleasure.MockStrictAsObject<ISessionFactory>());
                                  };

        Because of = () => { currentSession = sessionFactory.GetCurrent(); };

        It should_be_same_session = () => currentSession.ShouldBeTheSameAs(session);
    }
}