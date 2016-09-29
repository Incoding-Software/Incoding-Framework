namespace Incoding.Data
{
    #region << Using >>

    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

    #endregion

    [UsedImplicitly, ExcludeFromCodeCoverage]
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

        public IUnitOfWork Create(IsolationLevel level, bool isFlush, string connection = null)
        {
            return new EntityFrameworkUnitOfWork(sessionFactory.Open(connection), level,isFlush);
        }

        #endregion
    }
}