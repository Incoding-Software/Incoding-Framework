namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Data;

    #endregion

    public abstract class UnitOfWorkBase<TSession, TSessionFactory> : IUnitOfWork where TSessionFactory : ISessionFactory<TSession>
                                                                                  where TSession : IDisposable
    {
        #region Fields

        protected readonly TSessionFactory sessionFactory;

        protected readonly Lazy<TSession> session;

        protected readonly IsolationLevel isolationLevel;

        protected bool isWasCommit;

        bool disposed;

        #endregion

        #region Constructors

        protected UnitOfWorkBase(TSessionFactory sessionFactory, IsolationLevel isolationLevel, string connectionString)
        {
            this.isolationLevel = isolationLevel;
            this.sessionFactory = sessionFactory;
            this.session = new Lazy<TSession>(() => sessionFactory.Open(connectionString));
        }

        #endregion

        #region IUnitOfWork Members

        public bool IsOpen()
        {
            return !this.disposed && this.session.IsValueCreated;
        }

        public void Flush()
        {
            if (IsOpen())
                InternalFlush();
        }

        public void Commit()
        {
            if (IsOpen())
                InternalCommit();
            this.isWasCommit = true;
        }

        public void Open()
        {
            if (!IsOpen())
                InternalOpen();
        }

        public object GetSession()
        {
            return session;
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            if (IsOpen())
            {
                InternalSubmit();
                this.session.Value.Dispose();
            }

            this.disposed = true;
        }

        #endregion

        protected abstract void InternalSubmit();

        protected abstract void InternalFlush();

        protected abstract void InternalCommit();

        protected abstract void InternalOpen();
    }
}