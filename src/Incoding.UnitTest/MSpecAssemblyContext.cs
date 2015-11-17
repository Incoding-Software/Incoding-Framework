namespace Incoding.UnitTest
{
    #region << Using >>

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
    using NHibernate.Tool.hbm2ddl;

    #endregion

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
            return Fluently
                    .Configure()
                    .Database(MsSqlConfiguration.MsSql2008
                                                .ConnectionString(ConnectionString)
                                                .ShowSql())
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                    .Mappings(configuration => configuration.FluentMappings
                                                            .Add(typeof(DelayToScheduler.Map))
                                                            .AddFromAssembly(typeof(DbEntity).Assembly));
        }

        #endregion

        #region IAssemblyContext Members

        public void OnAssemblyStart()
        {
            var currentUiCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = currentUiCulture;
            Thread.CurrentThread.CurrentCulture = currentUiCulture;
            PleasureForData.StartNhibernate(() => NhibernateFluent());
        }

        public void OnAssemblyComplete() { }

        #endregion
    }

    ////ncrunch: no coverage end
}