namespace $rootnamespace$
{

    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using System.Configuration;
	using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;

	////ncrunch: no coverage start	
    public class MSpecAssemblyContext : IAssemblyContext
    {
        #region IAssemblyContext Members

        public void OnAssemblyStart()
        {
            PleasureForData.StartNhibernate(() =>
                                            {
                                                return Fluently
                                                        .Configure()
                                                        .Database(MsSqlConfiguration.MsSql2008
                                                                                    .ConnectionString(ConfigurationManager.ConnectionStrings["Test"].ConnectionString)
                                                                                    .ShowSql())
                                                        .Mappings(configuration => configuration.FluentMappings.AddFromAssembly(typeof(Bootstrapper).Assembly));
                                            }, true);
        }

        public void OnAssemblyComplete()
        {		  
        }

        #endregion
    }
    ////ncrunch: no coverage end
}