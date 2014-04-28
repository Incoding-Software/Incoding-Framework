namespace Incoding.Data
{
    #region << Using >>

    using System.Transactions;
    using Incoding.Extensions;
    using Raven.Client;
    using IsolationLevel = System.Data.IsolationLevel;

    #endregion

    public class RavenDbUnitOfWork : UnitOfWorkBase<IDocumentSession, IRavenDbSessionFactory>
    {
        #region Fields

        TransactionScope transaction;

        #endregion

        #region Constructors

        public RavenDbUnitOfWork(IRavenDbSessionFactory sessionFactory, string connection, IsolationLevel level)
                : base(sessionFactory, level, connection) { }

        #endregion

        protected override void InternalFlush()
        {
            this.session.Value.SaveChanges();
        }

        protected override void InternalCommit()
        {
            this.transaction.Complete();
        }

        protected override void InternalOpen()
        {
            this.transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
            this.session.Initialize();
        }

        protected override void InternalSubmit()
        {
            this.transaction.Dispose();
        }
    }
}