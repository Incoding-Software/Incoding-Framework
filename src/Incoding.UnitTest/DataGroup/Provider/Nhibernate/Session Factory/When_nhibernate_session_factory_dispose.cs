namespace Incoding.UnitTest
{
    using System.Reflection;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using NCrunch.Framework;
    using NHibernate;

    [Subject(typeof(NhibernateSessionFactory)), Isolated]
    public class When_nhibernate_session_factory_dispose
    {
        #region Establish value

        static NhibernateSessionFactory nhSessionFactory;

        #endregion

        Establish establish = () =>
                                  {
                                      var currentSession = Pleasure.MockStrictAsObject<ISession>();
                                      nhSessionFactory = new NhibernateSessionFactory(Pleasure.MockStrictAsObject<ISessionFactory>(mock => mock.Setup(r => r.OpenSession()).Returns(currentSession)));
                                      typeof(NhibernateSessionFactory).GetField("currentSession", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, currentSession);
                                  };

        Because of = () => nhSessionFactory.Dispose();

        It should_be_not_be_null = () => nhSessionFactory.TryGetValue("currentSession").ShouldBeNull();
    }
}