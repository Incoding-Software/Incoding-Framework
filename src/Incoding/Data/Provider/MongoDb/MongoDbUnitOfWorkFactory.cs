namespace Incoding.Data
{
    #region << Using >>

    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

    #endregion

    [UsedImplicitly, ExcludeFromCodeCoverage]
    public class MongoDbUnitOfWorkFactory : IUnitOfWorkFactory
    {
        #region Fields

        readonly IMongoDbSessionFactory sessionFactory;

        #endregion

        #region Constructors

        public MongoDbUnitOfWorkFactory(IMongoDbSessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        #endregion

        #region IUnitOfWorkFactory Members

        public IUnitOfWork Create(IsolationLevel level, string connection = null)
        {
            return new MongoDbUnitOfWork(this.sessionFactory, level, connection);
        }

        #endregion
    }
}