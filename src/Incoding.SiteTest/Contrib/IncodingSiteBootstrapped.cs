namespace Incoding.SiteTest.Contrib
{
    #region << Using >>

    using System.Configuration;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Incoding.Block;
    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using NHibernate.Context;
    using NHibernate.Tool.hbm2ddl;

    #endregion

    public static class IncodingSiteBootstrapped
    {
        #region Factory constructors

        public static void Start()
        {
            IoCFactory.Instance.Initialize(init => init.WithProvider(new StructureMapIoCProvider(registry =>
                                                                                                     {
                                                                                                         registry.For<IDispatcher>().Singleton().Use<DefaultDispatcher>();
                                                                                                         registry.For<IEventBroker>().Singleton().Use<DefaultEventBroker>();
                                                                                                         registry.For<ITemplateFactory>().Singleton().Use<TemplateDoTFactory>();

                                                                                                         var configure = Fluently
                                                                                                                 .Configure()
                                                                                                                 .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                                                                                                                 .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.ConnectionStrings["Main"].ConnectionString))
                                                                                                                 .Mappings(configuration => configuration.FluentMappings
                                                                                                                                                         .Add<DelayToScheduler.Map>()
                                                                                                                                                         .AddFromAssembly(typeof(IncodingSiteBootstrapped).Assembly));
                                                                                                                 
                                                                                                         registry.For<IManagerDataBase>().Singleton().Use(() => new NhibernateManagerDataBase(configure));
                                                                                                         registry.For<INhibernateSessionFactory>().Singleton().Use(() => new NhibernateSessionFactory(configure));
                                                                                                         registry.For<IUnitOfWorkFactory>().Singleton().Use<NhibernateUnitOfWorkFactory>();
                                                                                                         registry.For<IRepository>().Use<NhibernateRepository>();
                                                                                                     })));

            var managerDb = IoCFactory.Instance.TryResolve<IManagerDataBase>();
            if (!managerDb.IsExist())
                managerDb.Create();
        }

        #endregion
    }

}