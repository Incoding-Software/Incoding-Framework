namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Data;

    #endregion

    public abstract class UnitOfWorkBase<TSession, TSessionFactory> : IUnitOfWork
            where TSessionFactory : ISessionFactory<TSession>
            where TSession : IDisposable
    {
        #region Fields

        protected readonly Lazy<TSession> session;

        protected readonly IsolationLevel isolationLevel;

        bool disposed;

        bool isWasFlush;

        #endregion

        #region Constructors

        protected UnitOfWorkBase(TSessionFactory sessionFactory, IsolationLevel isolationLevel, string connectionString)
        {
            this.isolationLevel = isolationLevel;
            session = new Lazy<TSession>(() => sessionFactory.Open(connectionString));
        }

        #endregion

        #region IUnitOfWork Members

        public abstract IRepository GetRepository();

        public void Flush()
        {
            if (IsOpen())
            {
                InternalFlush();
                isWasFlush = true;
            }
        }

        public void Commit()
        {
            if (IsOpen() && isWasFlush)
                InternalCommit();
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            if (IsOpen())
            {
                InternalSubmit();
                session.Value.Dispose();
            }

            disposed = true;
        }

        #endregion

        protected abstract void InternalSubmit();

        protected abstract void InternalFlush();

        protected abstract void InternalCommit();

        bool IsOpen()
        {
            return !disposed && session.IsValueCreated;
        }
    }
}