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

        public IUnitOfWork Create(IsolationLevel level, string connection = null)
        {
            return new NhibernateUnitOfWork(this.sessionFactory, connection, level);
        }

        #endregion
    }
}