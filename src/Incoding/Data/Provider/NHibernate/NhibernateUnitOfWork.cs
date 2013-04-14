namespace Incoding.Data
{
    #region << Using >>

    using System.Data;
    using NHibernate;
    using NHibernate.Context;

    #endregion

    public class NhibernateUnitOfWork : IUnitOfWork
    {
        #region Fields

        readonly ISession session;

        readonly ITransaction transaction;

        #endregion

        #region Constructors

        public NhibernateUnitOfWork(ISession session, IsolationLevel isolationLevel)
        {
            Guard.NotNull("session", session);
            CurrentSessionContext.Bind(session);

            this.session = session;
            this.transaction = session.BeginTransaction(isolationLevel);
        }

        #endregion

        #region IUnitOfWork Members

        public void Commit()
        {
            this.session.Flush();
            this.transaction.Commit();
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            if (!this.transaction.WasCommitted && !this.transaction.WasRolledBack)
                this.transaction.Rollback();

            this.transaction.Dispose();

            CurrentSessionContext.Unbind(this.session.SessionFactory);
            this.session.Dispose();
        }

        #endregion
    }
}