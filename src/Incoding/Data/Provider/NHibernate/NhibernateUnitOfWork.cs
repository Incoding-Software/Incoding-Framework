namespace Incoding.Data
{
    #region << Using >>

    using System.Data;
    using NHibernate;

    #endregion

    public class NhibernateUnitOfWork : UnitOfWorkBase<ISession, INhibernateSessionFactory>
    {
        #region Fields

        ITransaction transaction;

        #endregion

        #region Constructors

        public NhibernateUnitOfWork(INhibernateSessionFactory sessionFactory, string connectionString, IsolationLevel isolationLevel)
                : base(sessionFactory, isolationLevel, connectionString) { }

        #endregion

        protected override void InternalFlush()
        {
            session.Value.Flush();
        }

        protected override void InternalCommit()
        {
            transaction.Commit();
        }

        protected override void InternalSubmit()
        {
            transaction.Dispose();
        }

        public override IRepository GetRepository()
        {
            if (transaction == null)
                transaction = session.Value.BeginTransaction(isolationLevel);
            return new NhibernateRepository(session.Value);
        }
    }
}