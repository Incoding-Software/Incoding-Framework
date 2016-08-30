namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using NCrunch.Framework;
    using NHibernate;

    #endregion

    [Subject(typeof(NhibernateSessionFactory)), Isolated]
    public class When_nhibernate_session_factory_open_session
    {
        #region Establish value

        static ISession currentSession;

        static NhibernateSessionFactory nhSessionFactory;

        #endregion

        Establish establish = () =>
                                  {
                                      var currentSession = Pleasure.MockStrictAsObject<ISession>();
                                      nhSessionFactory = new NhibernateSessionFactory(Pleasure.MockStrictAsObject<ISessionFactory>(mock => mock.Setup(r => r.OpenSession()).Returns(currentSession)));
                                  };

        Because of = () => { currentSession = nhSessionFactory.Open(null); };

        It should_be_not_be_null = () => currentSession.ShouldBeTheSameAs(currentSession); //-V3062
    }
}