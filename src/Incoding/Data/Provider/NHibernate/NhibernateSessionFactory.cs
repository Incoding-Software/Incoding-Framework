namespace Incoding.Data
{
    #region << Using >>

    using System;
    using FluentNHibernate.Cfg;
    using NHibernate;

    #endregion

    public class NhibernateSessionFactory : INhibernateSessionFactory
    {
        #region Static Fields

        [ThreadStatic]
        static ISession currentSession;

        #endregion

        #region Fields

        readonly Lazy<ISessionFactory> sessionFactory;

        #endregion

        ////ncrunch: no coverage start
        #region Constructors

        public NhibernateSessionFactory(FluentConfiguration fluentConfiguration)
        {
            this.sessionFactory = new Lazy<ISessionFactory>(fluentConfiguration.BuildSessionFactory);
        }

        ////ncrunch: no coverage end
        public NhibernateSessionFactory(ISessionFactory sessionFactory)
        {
            this.sessionFactory = new Lazy<ISessionFactory>(() => sessionFactory);
        }

        #endregion

        #region INhibernateSessionFactory Members

        public ISession GetCurrent()
        {
            if (currentSession != null)
                return currentSession;

            throw new InvalidOperationException("Database access logic cannot be used, if session not opened. Implicitly session usage not allowed now. Please open session explicitly through UnitOfWorkFactory.Create method");
        }

        public ISession Open(string connectionString)
        {
            var session = this.sessionFactory.Value.OpenSession();
            if (!string.IsNullOrWhiteSpace(connectionString))
                session.Connection.ConnectionString = connectionString;

            currentSession = session;
            return currentSession;
        }

        public void Dispose()
        {
            currentSession = null;
        }

        #endregion
    }
}