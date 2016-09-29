namespace Incoding.Data
{
    #region << Using >>

    using System.Data;
    using Raven.Client;
    using IsolationLevel = System.Data.IsolationLevel;

    #endregion

    public class RavenDbUnitOfWorkFactory
    {
        #region Fields

        readonly IRavenDbSessionFactory sessionFactory;

        #endregion

        #region Constructors

        public RavenDbUnitOfWorkFactory(IRavenDbSessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        #endregion

        #region IUnitOfWorkFactory Members

        public IUnitOfWork Create(IsolationLevel level,  string connection = null)
        {
            return new RavenDbUnitOfWork(sessionFactory.Open(connection), level);
        }

        #endregion
    }
}   