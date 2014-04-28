namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Data.Entity;

    #endregion

    public class EntityFrameworkSessionFactory : IEntityFrameworkSessionFactory
    {
        #region Static Fields

        [ThreadStatic]
        static DbContext currentSession;

        #endregion

        #region Fields

        readonly Func<DbContext> createDb;

        #endregion

        #region Constructors

        public EntityFrameworkSessionFactory(Func<DbContext> createDb)
        {
            this.createDb = createDb;
        }

        #endregion

        #region IEntityFrameworkSessionFactory Members

        public DbContext GetCurrent()
        {
            if (currentSession != null)
                return currentSession;

            throw new InvalidOperationException(SpecificationMessageRes.Session_Factory_Not_Open);
        }

        public DbContext Open(string connectionString)
        {
            currentSession = this.createDb();
            if (!string.IsNullOrWhiteSpace(connectionString))
                currentSession.Database.Connection.ConnectionString = connectionString;
            return currentSession;
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            currentSession = null;
        }

        #endregion
    }
}