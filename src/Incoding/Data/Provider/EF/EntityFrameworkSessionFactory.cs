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

        public DbContext Open(string connectionString)
        {
            currentSession = this.createDb();
            if (!string.IsNullOrWhiteSpace(connectionString))
                currentSession.Database.Connection.ConnectionString = connectionString;
            return currentSession;
        }

        #endregion
    }
}