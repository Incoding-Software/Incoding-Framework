namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Data;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using NCrunch.Framework;
    using NHibernate;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(NhibernateUnitOfWork)), Isolated]
    public class When_nhibernate_unit_of_work
    {
        #region Establish value

        static void Run(Action<NhibernateUnitOfWork, Mock<ISession>, Mock<ITransaction>> action, bool isFlush = true)
        {
            var transaction = Pleasure.Mock<ITransaction>();

            var session = Pleasure.Mock<ISession>(mock => mock.Setup(r => r.BeginTransaction(IsolationLevel.RepeatableRead)).Returns(transaction.Object));

            var nhibernateUnit = new NhibernateUnitOfWork(session.Object, IsolationLevel.RepeatableRead, isFlush);
            session.Verify(r => r.BeginTransaction(IsolationLevel.RepeatableRead), Times.Once());
            action(nhibernateUnit, session, transaction);
        }

        #endregion

        It should_be_dispose = () => Run((work, session, transaction) =>
                                         {
                                             work.Dispose();
                                             session.Verify(r => r.Dispose(), Times.Once());
                                             transaction.Verify(r => r.Dispose(), Times.Once());
                                             transaction.Verify(r => r.Commit(), Times.Never());
                                             work.TryGetValue<bool>("disposed").ShouldBeTrue();
                                         });

        It should_be_dispose_after_flush = () => Run((work, session, transaction) =>
                                                     {
                                                         work.Flush();
                                                         work.Dispose();

                                                         session.Verify(r => r.Dispose(), Times.Once());
                                                         transaction.Verify(r => r.Dispose(), Times.Once());
                                                         transaction.Verify(r => r.Commit(), Times.Once());
                                                         work.TryGetValue<bool>("disposed").ShouldBeTrue();
                                                         work.TryGetValue("isWasFlush").ShouldEqual(true);
                                                     });

        It should_be_flush_without_flush = () => Run((work, session, transaction) =>
                                                     {
                                                         work.Flush();

                                                         session.Verify(r => r.Flush(), Times.Never());
                                                         work.TryGetValue("isWasFlush").ShouldEqual(false);
                                                     }, isFlush: false);

        It should_be_flush = () => Run((work, session, transaction) =>
                                       {
                                           work.Flush();

                                           session.Verify(r => r.Flush(), Times.Once());
                                           work.TryGetValue("isWasFlush").ShouldEqual(true);
                                       });

        It should_be_get_repository = () => Run((work, session, transaction) => work.GetRepository().ShouldBeOfType<NhibernateRepository>());
    }
}