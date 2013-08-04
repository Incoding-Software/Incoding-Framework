namespace $rootnamespace$
{

    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using System.Configuration;
	using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;


    public class MSpecAssemblyContext : IAssemblyContext
    {
        #region IAssemblyContext Members

        public void OnAssemblyStart()
        {
		            var configure = Fluently
                    .Configure()
                    .Database(MsSqlConfiguration.MsSql2008
                                                .ConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString)
                                                .ShowSql())
                    .Mappings(configuration => configuration.FluentMappings.AddFromAssembly(typeof(Bootstrapper).Assembly));

            NHibernatePleasure.StartSession(configure, true);
        }

        public void OnAssemblyComplete()
        {
		  NHibernatePleasure.StopAllSession();
        }

        #endregion
    }

}