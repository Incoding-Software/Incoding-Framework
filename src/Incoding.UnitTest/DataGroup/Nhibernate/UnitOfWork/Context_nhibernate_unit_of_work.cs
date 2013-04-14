namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Moq;
    using NHibernate;
    using NHibernate.Context;
    using NHibernate.Engine;

    #endregion

    public class Context_nhibernate_unit_of_work
    {
        #region Estabilish value

        protected static Mock<ISession> session;

        protected static NhibernateUnitOfWork nhibernateUnit;

        protected static Mock<ITransaction> transaction;

        #endregion

        #region Constructors

        protected Context_nhibernate_unit_of_work()
        {
            session = Pleasure.Mock<ISession>();

            var sessionFactoryImplementor = Pleasure.MockAsObject<ISessionFactoryImplementor>(mock => mock
                                                                                                              .SetupGet(r => r.CurrentSessionContext)
                                                                                                              .Returns(Pleasure.MockAsObject<CurrentSessionContext>()));
            session
                    .Setup(r => r.SessionFactory)
                    .Returns(sessionFactoryImplementor);

            const IsolationLevel isolationLevel = IsolationLevel.ReadCommitted;
            transaction = Pleasure.Mock<ITransaction>();
            session
                    .Setup(r => r.BeginTransaction(isolationLevel))
                    .Returns(transaction.Object);

            nhibernateUnit = new NhibernateUnitOfWork(session.Object, isolationLevel);
        }

        #endregion
    }
}