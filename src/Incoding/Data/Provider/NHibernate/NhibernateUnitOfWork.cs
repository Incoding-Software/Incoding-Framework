namespace Incoding.Data
{
    #region << Using >>

    using System.Data;
    using NHibernate;

    #endregion

    public class NhibernateUnitOfWork : UnitOfWorkBase<ISession>
    {
        #region Fields

        readonly ITransaction transaction;

        #endregion

        #region Constructors

        public NhibernateUnitOfWork(ISession session, IsolationLevel isolationLevel, bool isFlush)
                : base(session)
        {
            transaction = session.BeginTransaction(isolationLevel);
            bool isReadonly = !isFlush;
            session.DefaultReadOnly = isReadonly;
            if (isReadonly)
                session.FlushMode = FlushMode.Never;
            repository = new NhibernateRepository(session);
        }

        #endregion

        protected override void InternalFlush()
        {
            session.Flush();
        }

        protected override void InternalCommit()
        {
            transaction.Commit();
        }

        protected override void InternalSubmit()
        {
            transaction.Dispose();
        }
    }
}