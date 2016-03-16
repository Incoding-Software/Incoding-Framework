namespace Incoding.Data
{
    #region << Using >>

    using NHibernate;

    #endregion

    public interface INhibernateSessionFactory : ISessionFactory<ISession> {        
    }
}