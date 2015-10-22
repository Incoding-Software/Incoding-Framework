namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Data;

    #endregion

    public abstract class UnitOfWorkBase<TSession> : IUnitOfWork
            where TSession : class, IDisposable
    {
        #region Fields

        protected readonly TSession session;
        
        protected readonly bool isFlush;

        protected IRepository repository;

        bool disposed;

        bool isWasFlush;

        #endregion

        #region Constructors

        protected UnitOfWorkBase(TSession session, IsolationLevel isolationLevel, bool isFlush)
        {            
            this.isFlush = isFlush;
            this.session = session;
        }

        #endregion

        #region IUnitOfWork Members

        public IRepository GetRepository()
        {
            return repository;
        }

        public void Flush()
        {
            if (!disposed && isFlush)
            {
                InternalFlush();
                isWasFlush = true;
            }
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            if (!disposed)
            {
                if (isWasFlush)
                    InternalCommit();

                InternalSubmit();
                session.Dispose();
            }

            disposed = true;
        }

        #endregion

        protected abstract void InternalSubmit();

        protected abstract void InternalFlush();

        protected abstract void InternalCommit();
    }
}