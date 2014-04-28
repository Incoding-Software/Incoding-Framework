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

        #endregion

        #region Constructors

        public EntityFrameworkUnitOfWork(IEntityFrameworkSessionFactory sessionFactory, IsolationLevel level, string connection)
                : base(sessionFactory, level, connection) { }

        #endregion

        protected override void InternalFlush()
        {
            this.session.Value.SaveChanges();
        }

        protected override void InternalOpen()
        {
            this.transaction = this.session.Value.Database.BeginTransaction(this.isolationLevel);
        }

        protected override void InternalCommit()
        {
            this.transaction.Commit();
        }

        protected override void InternalSubmit()
        {
            if (!this.isWasCommit)
                this.transaction.Rollback();

            this.transaction.Dispose();
        }
    }
}