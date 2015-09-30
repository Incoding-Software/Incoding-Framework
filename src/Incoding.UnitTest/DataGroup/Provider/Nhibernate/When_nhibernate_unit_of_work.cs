namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Data;
    using System.Data.SqlClient;
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

        static void Run(Action<NhibernateUnitOfWork, Mock<ISession>, Mock<ITransaction>> action)
        {
            var transaction = Pleasure.Mock<ITransaction>();

            string dbConnection = Pleasure.Generator.Invent<SqlConnection>().ConnectionString;
            var session = Pleasure.Mock<ISession>(mock => mock.Setup(r => r.BeginTransaction(IsolationLevel.RepeatableRead)).Returns(transaction.Object));

            var sessionFactory = Pleasure.MockStrictAsObject<INhibernateSessionFactory>(mock => mock.Setup(r => r.Open(dbConnection)).Returns(session.Object));
            var nhibernateUnit = new NhibernateUnitOfWork(sessionFactory, dbConnection, IsolationLevel.RepeatableRead);

            action(nhibernateUnit, session, transaction);
        }

        #endregion

        It should_be_commit_without_flush = () => Run((work, session, transaction) =>
                                                      {
                                                          var repository = work.GetRepository();
                                                          work.Commit();
                                                          transaction.Verify(r => r.Commit(), Times.Never());
                                                      });

        It should_be_commit = () => Run((work, session, transaction) =>
                                        {
                                            var repository = work.GetRepository();
                                            work.Flush();
                                            work.Commit();
                                            transaction.Verify(r => r.Commit(), Times.Once());
                                        });

        It should_be_dispose_without_open = () => Run((work, session, transaction) =>
                                                      {
                                                          work.Dispose();
                                                          session.Verify(r => r.Dispose(), Times.Never());
                                                          transaction.Verify(r => r.Dispose(), Times.Never());
                                                          work.TryGetValue<bool>("disposed").ShouldBeTrue();
                                                      });

        It should_be_dispose = () => Run((work, session, transaction) =>
                                         {
                                             var repository = work.GetRepository();
                                             work.Dispose();
                                             session.Verify(r => r.Dispose(), Times.Once());
                                             transaction.Verify(r => r.Dispose(), Times.Once());
                                             work.TryGetValue<bool>("disposed").ShouldBeTrue();
                                         });

        It should_be_flush = () => Run((work, session, transaction) =>
                                       {
                                           var repository = work.GetRepository();
                                           session.Verify(r => r.BeginTransaction(IsolationLevel.RepeatableRead), Times.Once());
                                           work.Flush();
                                           session.Verify(r => r.Flush(), Times.Once());
                                       });

        It should_be_flush_without_open = () => Run((work, session, transaction) =>
                                                    {
                                                        work.Flush();
                                                        session.Verify(r => r.Flush(), Times.Never());
                                                    });

        It should_be_get_repository = () => Run((work, session, transaction) =>
                                                {
                                                    var repository = work.GetRepository();
                                                    session.Verify(r => r.BeginTransaction(IsolationLevel.RepeatableRead), Times.Once());
                                                    repository.ShouldBeOfType<NhibernateRepository>();
                                                });
    }
}