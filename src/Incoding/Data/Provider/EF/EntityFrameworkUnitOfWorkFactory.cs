namespace Incoding.Data
{
    #region << Using >>

    using System.Data;

    #endregion

    public class EntityFrameworkUnitOfWorkFactory : IUnitOfWorkFactory
    {
        #region Fields

        readonly IEntityFrameworkSessionFactory sessionFactory;

        #endregion

        #region Constructors

        public EntityFrameworkUnitOfWorkFactory(IEntityFrameworkSessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        #endregion

        #region IUnitOfWorkFactory Members

        public IUnitOfWork Create(IsolationLevel level, string connection = null)
        {
            return new EntityFrameworkUnitOfWork(this.sessionFactory, level, connection);
        }

        #endregion
    }
}