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
            this.session.Value.Flush();
        }

        protected override void InternalCommit()
        {
            this.transaction.Commit();
        }

        protected override void InternalOpen()
        {
            this.transaction = this.session.Value.BeginTransaction(this.isolationLevel);
        }

        protected override void InternalSubmit()
        {
            if (!this.transaction.WasCommitted && !this.transaction.WasRolledBack)
                this.transaction.Rollback();

            this.transaction.Dispose();
        }
    }
}