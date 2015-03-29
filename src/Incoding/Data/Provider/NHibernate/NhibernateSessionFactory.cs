namespace Incoding.Data
{
    #region << Using >>

    using System;
    using FluentNHibernate.Cfg;
    using NHibernate;

    #endregion

    public class NhibernateSessionFactory : INhibernateSessionFactory
    {
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

        public ISession Open(string connectionString)
        {
            var session = this.sessionFactory.Value.OpenSession();
            if (!string.IsNullOrWhiteSpace(connectionString))
                session.Connection.ConnectionString = connectionString;

            return session;
        }

        #endregion
    }
}