namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Reflection;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using NCrunch.Framework;
    using NHibernate;

    #endregion

    [Subject(typeof(NhibernateSessionFactory)), Isolated]
    public class When_nhibernate_session_factory_get_current_session_without_open_session
    {
        #region Establish value

        static InvalidOperationException exception;

        static NhibernateSessionFactory nhSessionFactory;

        #endregion

        Establish establish = () =>
                                  {
                                      nhSessionFactory = new NhibernateSessionFactory(Pleasure.MockStrictAsObject<ISessionFactory>());
                                      typeof(NhibernateSessionFactory).GetField("currentSession", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, null);
                                  };

        Because of = () => { exception = Catch.Exception(() => nhSessionFactory.GetCurrent()) as InvalidOperationException; };

        It should_be_exception = () => exception.ShouldNotBeNull();
    }
}