namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Configuration;
    using System.Globalization;
    using System.Threading;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;
    using NHibernate.Context;

    #endregion

    ////ncrunch: no coverage start
    [UsedImplicitly]
    public class MSpecAssemblyContext : IAssemblyContext
    {
        #region IAssemblyContext Members

        public void OnAssemblyStart()
        {
            var currentUiCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = currentUiCulture;
            Thread.CurrentThread.CurrentCulture = currentUiCulture;

            var msSql = Fluently
                    .Configure()
                    .Database(MsSqlConfiguration.MsSql2008
                                                .ConnectionString(ConfigurationManager.ConnectionStrings["IncRealDb"].ConnectionString)
                                                .ShowSql())
                    .Mappings(configuration => configuration.FluentMappings.AddFromAssembly(typeof(RealDbEntity).Assembly))
                    .CurrentSessionContext<ThreadStaticSessionContext>();

            NHibernatePleasure.StartSession(msSql, true);
        }

        public void OnAssemblyComplete()
        {
            NHibernatePleasure.StopAllSession();
        }

        #endregion
    }

    ////ncrunch: no coverage end
}