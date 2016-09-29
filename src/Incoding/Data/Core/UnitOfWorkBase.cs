namespace Incoding.Data
{
    #region << Using >>

    using System;

    #endregion

    public abstract class UnitOfWorkBase<TSession> : IUnitOfWork
            where TSession : class, IDisposable
    {
        #region Constructors

        protected UnitOfWorkBase(TSession session)
        {
            this.session = session;
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            if (!disposed)
            {
                InternalSubmit();
                session.Dispose();
            }

            disposed = true;
        }

        #endregion

        protected abstract void InternalSubmit();

        protected abstract void InternalFlush();

        protected abstract void InternalCommit();

        #region Fields

        protected readonly TSession session;

        protected IRepository repository;

        bool disposed;

        #endregion

        #region IUnitOfWork Members

        public IRepository GetRepository()
        {
            return repository;
        }

        public void Commit()
        {
            InternalCommit();
        }

        public void Flush()
        {
            if (!disposed)
                InternalFlush();
        }

        #endregion
    }
}