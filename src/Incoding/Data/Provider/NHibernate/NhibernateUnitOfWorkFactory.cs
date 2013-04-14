namespace Incoding.Data
{
    #region << Using >>

    using System.Data;

    #endregion

    public class NhibernateUnitOfWorkFactory : IUnitOfWorkFactory
    {
        #region Fields

        readonly INhibernateSessionFactory sessionFactory;

        #endregion

        #region Constructors

        public NhibernateUnitOfWorkFactory(INhibernateSessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        #endregion

        #region IUnitOfWorkFactory Members

        public IUnitOfWork Create(IsolationLevel level, IDbConnection connection = null)
        {
            return new NhibernateUnitOfWork(this.sessionFactory.OpenSession(connection), level);
        }

        #endregion
    }
}