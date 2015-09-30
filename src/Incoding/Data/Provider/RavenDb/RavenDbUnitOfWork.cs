namespace Incoding.Data
{
    #region << Using >>

    using System.Transactions;
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
            session.Value.SaveChanges();
        }

        protected override void InternalCommit()
        {
            transaction.Complete();
        }

        protected override void InternalSubmit()
        {
            transaction.Dispose();
        }

        public override IRepository GetRepository()
        {
            if (transaction == null)
                transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
            return new RavenDbRepository(session.Value);
        }
    }
}