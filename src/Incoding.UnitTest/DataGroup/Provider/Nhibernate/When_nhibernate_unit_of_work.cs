namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Data;
    using System.Data.SqlClient;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using NCrunch.Framework;
    using NHibernate;
    using NHibernate.Context;
    using NHibernate.Engine;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(NhibernateUnitOfWork)), Isolated]
    public class When_nhibernate_unit_of_work
    {
        #region Establish value

        static void Run(Action<NhibernateUnitOfWork, Mock<ISession>, Mock<ITransaction>> action, bool isOpen = true, bool isDispose = false)
        {
            var transaction = Pleasure.Mock<ITransaction>();

            string dbConnection = Pleasure.Generator.Invent<SqlConnection>().ConnectionString;
            var session = Pleasure.Mock<ISession>(mock =>
                                                      {
                                                          mock.Setup(r => r.SessionFactory).Returns(Pleasure.MockAsObject<ISessionFactoryImplementor>(r => r.SetupGet(implementor => implementor.CurrentSessionContext)
                                                                                                                                                            .Returns(Pleasure.MockAsObject<CurrentSessionContext>())));
                                                          mock.Setup(r => r.BeginTransaction(IsolationLevel.RepeatableRead)).Returns(transaction.Object);
                                                      });

            var nhibernateUnit = new NhibernateUnitOfWork(Pleasure.MockStrictAsObject<INhibernateSessionFactory>(mock =>
                                                                                                                     {
                                                                                                                         mock.Setup(r => r.Open(dbConnection)).Returns(session.Object);
                                                                                                                     }), dbConnection, IsolationLevel.RepeatableRead);
            if (isOpen)
                nhibernateUnit.Open();

            action(nhibernateUnit, session, transaction);
        }

        #endregion

        It should_be_flush = () => Run((work, session, transaction) =>
                                           {
                                               work.Flush();
                                               session.Verify(r => r.Flush());
                                           });

        It should_be_open = () => Run((work, session, transaction) => session.Verify(r => r.BeginTransaction(IsolationLevel.RepeatableRead), Times.Once()));

        It should_be_is_open = () => Run((work, session, transaction) => work.IsOpen().ShouldBeTrue());

        It should_be_is_open_after_dispose = () => Run((work, session, transaction) =>
                                                           {
                                                               work.Dispose();
                                                               work.IsOpen().ShouldBeFalse();
                                                           }, isDispose: true);

        It should_be_not_is_open = () => Run((work, session, transaction) => work.IsOpen().ShouldBeFalse(), isOpen: false);

        It should_be_commit = () => Run((work, session, transaction) =>
                                            {
                                                work.Commit();
                                                session.Verify(r => r.Flush(), Times.Never());
                                                transaction.Verify(r => r.Commit(), Times.Once());
                                            });

        It should_be_dispose = () => Run((work, session, transaction) =>
                                             {
                                                 work.Dispose();
                                                 session.Verify(r => r.Dispose());
                                                 transaction.Verify(r => r.Dispose());
                                             }, isDispose: true);

        It should_be_dispose_with_rollback = () => Run((work, session, transaction) =>
                                                           {
                                                               transaction.SetupGet(r => r.WasCommitted).Returns(false);
                                                               transaction.SetupGet(r => r.WasRolledBack).Returns(false);

                                                               work.Dispose();

                                                               transaction.Verify(r => r.Rollback());
                                                               session.Verify(r => r.Dispose());
                                                               transaction.Verify(r => r.Dispose());
                                                           }, isDispose: true);
    }
}