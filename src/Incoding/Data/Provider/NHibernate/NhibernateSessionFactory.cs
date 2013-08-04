namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Data;
    using FluentNHibernate.Cfg;
    using NHibernate;
    using NHibernate.Context;

    #endregion

    public class NhibernateSessionFactory : INhibernateSessionFactory
    {
        #region Fields

        readonly Lazy<ISessionFactory> sessionFactory;

        #endregion

        #region Constructors

        public NhibernateSessionFactory(FluentConfiguration fluentConfiguration)
        {
            Guard.NotNull("fluentConfiguration", fluentConfiguration);
            this.sessionFactory = new Lazy<ISessionFactory>(fluentConfiguration.BuildSessionFactory);
        }

        #endregion

        #region INhibernateSessionFactory Members

        public ISession GetCurrentSession()
        {
            if (CurrentSessionContext.HasBind(this.sessionFactory.Value))
                return this.sessionFactory.Value.GetCurrentSession();

            ////ncrunch: no coverage start
            throw new InvalidOperationException("Database access logic cannot be used, if session not opened. Implicitly session usage not allowed now. Please open session explicitly through UnitOfWorkFactory.Create method");

            ////ncrunch: no coverage end
        }

        public ISession OpenSession(IDbConnection connectionString)
        {
            if (connectionString == null)
                return this.sessionFactory.Value.OpenSession();

            if (connectionString.State != ConnectionState.Open)
                connectionString.Open();

            return this.sessionFactory.Value.OpenSession(connectionString);
        }

        #endregion
    }
}