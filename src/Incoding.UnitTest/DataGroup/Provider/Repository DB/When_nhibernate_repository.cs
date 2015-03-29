using System;

namespace Incoding.UnitTest
{
    #region << Using >>

    using FluentNHibernate;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using NCrunch.Framework;
    using NHibernate;
    using NHibernate.Context;

    #endregion

    [Subject(typeof(NhibernateRepository)), Isolated]
    public class When_nhibernate_repository : Behavior_repository
    {
        #region Establish value

        protected static IRepository GetRepository()
        {
            var openSession = MSpecAssemblyContext.NhibernateFluent<CallSessionContext>().BuildSessionFactory().OpenSession();
            var nhibernateRepository = new NhibernateRepository(/*Pleasure.MockStrictAsObject<INhibernateSessionFactory>(mock => mock.Setup(r => r.GetCurrent()).Returns(openSession))*/);
            nhibernateRepository.SetProvider(new Lazy<ISession>(() => openSession));
            return nhibernateRepository;
        }

        #endregion

        Behaves_like<Behavior_repository> should_be_behavior;

        Because of = () =>
                         {
                             repository = GetRepository();
                             repository.Init();
                         };
    }
}