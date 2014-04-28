namespace $rootnamespace$
{ 
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentValidation;
    using FluentValidation.Mvc;
    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using NHibernate.Context;

    public static class Bootstrapper
    {
        public static void Start()
        {
            LoggingFactory.Instance.Initialize(logging =>
                                                   {
                                                       string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                                                       logging.WithPolicy(policy => policy.For(LogType.Debug).Use(FileLogger.WithAtOnceReplace(path, () => "Debug_{0}.txt".F(DateTime.Now.ToString("yyyyMMdd")))));
                                                   });

            IoCFactory.Instance.Initialize(init => init.WithProvider(new StructureMapIoCProvider(registry =>
                                                                                                     {
                                                                                                         registry.For<IDispatcher>().Singleton().Use<DefaultDispatcher>();
                                                                                                         registry.For<IEventBroker>().Singleton().Use<DefaultEventBroker>();
                                                                                                         registry.For<ITemplateFactory>().Singleton().Use<TemplateHandlebarsFactory>();

                                                                                                         var configure = Fluently
                                                                                                                 .Configure()
                                                                                                                 .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.ConnectionStrings["Main"].ConnectionString))
                                                                                                                 .Mappings(configuration => configuration.FluentMappings.AddFromAssembly(typeof(Bootstrapper).Assembly))
                                                                                                                 .CurrentSessionContext<ThreadStaticSessionContext>();
                                                                                                         registry.For<IManagerDataBase>().Singleton().Use(() => new NhibernateManagerDataBase(configure));
                                                                                                         registry.For<INhibernateSessionFactory>().Singleton().Use(() => new NhibernateSessionFactory(configure));
                                                                                                         registry.For<IUnitOfWorkFactory>().Singleton().Use<NhibernateUnitOfWorkFactory>();
                                                                                                         registry.For<IRepository>().Use<NhibernateRepository>();

                                                                                                         registry.Scan(r =>
                                                                                                                           {
                                                                                                                               r.AssembliesFromApplicationBaseDirectory(p => p.GetTypes().Any(type => type.IsImplement(typeof(AbstractValidator<>)) ||
                                                                                                                                                                                                      type.IsImplement<ISetUp>() ||
                                                                                                                                                                                                      type.IsImplement(typeof(IEventSubscriber<>))));
                                                                                                                               r.WithDefaultConventions();

                                                                                                                               r.ConnectImplementationsToTypesClosing(typeof(AbstractValidator<>));
                                                                                                                               r.ConnectImplementationsToTypesClosing(typeof(IEventSubscriber<>));
                                                                                                                               r.AddAllTypesOf<ISetUp>();
                                                                                                                           });
                                                                                                     })));

            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new IncValidatorFactory()));
            FluentValidationModelValidatorProvider.Configure();

            foreach (var setUp in IoCFactory.Instance.ResolveAll<ISetUp>().OrderBy(r => r.GetOrder()))
                setUp.Execute();

            var ajaxDef = JqueryAjaxOptions.Default;
            ajaxDef.Cache = false; // disabled cache as default
        }
    }

}