namespace Incoding.Data
{
    #region << Using >>

    using System.Data;
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;

    #endregion

    [ExcludeFromCodeCoverage]
    public class EntityFrameworkUnitOfWork : UnitOfWorkBase<DbContext, IEntityFrameworkSessionFactory>
    {
        #region Fields

        DbContextTransaction transaction;

        bool isWasCommit;

        #endregion

        #region Constructors

        public EntityFrameworkUnitOfWork(IEntityFrameworkSessionFactory sessionFactory, IsolationLevel level, string connection)
                : base(sessionFactory, level, connection) { }

        #endregion

        protected override void InternalFlush()
        {
            session.Value.SaveChanges();
        }

        protected override void InternalCommit()
        {
            transaction.Commit();
            isWasCommit = true;
        }

        protected override void InternalSubmit()
        {
            if (!isWasCommit)
                transaction.Rollback();

            transaction.Dispose();
        }

        public override IRepository GetRepository()
        {
            if (transaction == null)
                transaction = session.Value.Database.BeginTransaction(isolationLevel);

            return new EntityFrameworkRepository(session.Value);
        }
    }
}