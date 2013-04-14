namespace Incoding.Data
{
    #region << Using >>

    using System.Data;
    using NHibernate;

    #endregion

    public interface INhibernateSessionFactory
    {
        #region Api Methods

        ISession GetCurrentSession();

        ISession OpenSession(IDbConnection connectionString);

        #endregion
    }
}