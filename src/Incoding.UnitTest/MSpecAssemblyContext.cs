namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Configuration;
    using System.Globalization;
    using System.Threading;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Incoding.Block;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;
    using NHibernate.Context;

    #endregion

    ////ncrunch: no coverage start
    [UsedImplicitly]
    public class MSpecAssemblyContext : IAssemblyContext
    {
        #region Static Fields

        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["IncRealNhibernateDb"].ConnectionString;

        #endregion

        #region Factory constructors

        public static FluentConfiguration NhibernateFluent()
        {
            return NhibernateFluent<CallSessionContext>();
        }

        public static FluentConfiguration NhibernateFluent<TContext>() where TContext : ICurrentSessionContext
        {
            return Fluently
                    .Configure()
                    .Database(MsSqlConfiguration.MsSql2008
                                                .ConnectionString(ConnectionString)
                                                .ShowSql())
                    .Mappings(configuration => configuration.FluentMappings
                                                            .Add(typeof(DelayToScheduler.Map))
                                                            .AddFromAssembly(typeof(DbEntity).Assembly))
                    .CurrentSessionContext<TContext>();
        }

        #endregion

        #region IAssemblyContext Members

        public void OnAssemblyStart()
        {
            var currentUiCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = currentUiCulture;
            Thread.CurrentThread.CurrentCulture = currentUiCulture;
            PleasureForData.StartNhibernate(NhibernateFluent<ThreadStaticSessionContext>());
        }

        public void OnAssemblyComplete() { }

        #endregion
    }

    ////ncrunch: no coverage end
}