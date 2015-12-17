namespace Incoding.SiteTest.Contrib
{
    #region << Using >>

    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using DryIoc;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentValidation;
    using FluentValidation.Mvc;
    using Incoding.Block;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using NHibernate.Tool.hbm2ddl;

    #endregion

    public static class IncodingSiteBootstrapped
    {
        #region Factory constructors

        public static void Start()
        {
            //IoCFactory.Instance.Initialize(init => init.WithProvider(new StructureMapIoCProvider(registry =>
            //                                                                                         {
            //                                                                                             registry.For<IDispatcher>().Use<DefaultDispatcher>();
            //                                                                                             registry.For<IEventBroker>().Use<DefaultEventBroker>();
            //                                                                                             registry.For<ITemplateFactory>().Use<TemplateDoTFactory>();

            //                                                                                             var configure = Fluently
            //                                                                                                     .Configure()
            //                                                                                                     .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
            //                                                                                                     .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.ConnectionStrings["Main"].ConnectionString))
            //                                                                                                     .Mappings(configuration => configuration.FluentMappings
            //                                                                                                                                             .Add<DelayToScheduler.Map>()
            //                                                                                                                                             .AddFromAssembly(typeof(IncodingSiteBootstrapped).Assembly));

            //                                                                                             registry.For<IManagerDataBase>().Singleton().Use(() => new NhibernateManagerDataBase(configure));
            //                                                                                             registry.For<INhibernateSessionFactory>().Singleton().Use(() => new NhibernateSessionFactory(configure));
            //                                                                                             registry.For<IUnitOfWorkFactory>().Use<NhibernateUnitOfWorkFactory>();                                                                                                         
            //                                                                                         })));
            var registry = new Container(scopeContext: new AsyncExecutionFlowScopeContext(),
                                         rules: Rules.Default.WithDefaultReuseInsteadOfTransient(Reuse.Transient));
            registry.Register<IDispatcher, DefaultDispatcher>();
            registry.Register<ITemplateFactory, TemplateDoTFactory>();

            var configure = Fluently
                    .Configure()
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.ConnectionStrings["Main"].ConnectionString))
                    .Mappings(configuration => configuration.FluentMappings
                                                            .Add<DelayToScheduler.Map>()
                                                            .AddFromAssembly(typeof(IncodingSiteBootstrapped).Assembly));

            registry.RegisterInstance<INhibernateSessionFactory>(new NhibernateSessionFactory(configure), reuse: Reuse.Singleton);
            registry.Register<IUnitOfWorkFactory, NhibernateUnitOfWorkFactory>(made: FactoryMethod.ConstructorWithResolvableArguments);

            foreach (var implementingClass in Assembly.GetExecutingAssembly()
                                                      .GetTypes()
                                                      .Where(type => type.IsImplement(typeof(AbstractValidator<>))))
            {
                var factory = new ReflectionFactory(implementingClass, Reuse.Singleton, Made.Default, Setup.Default);
                registry.Register(implementingClass.GetBaseType(), factory);
            }
            IoCFactory.Instance.Initialize(init => init.WithProvider(new DryIocProvider(registry)));

            FluentValidationModelValidatorProvider.Configure();
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new IncValidatorFactory()));

            IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v3;

            IoCFactory.Instance.TryResolve<IDispatcher>().Push(new StartSchedulerCommand()
                                                               {
                                                                       FetchSize = 10
                                                               });
        }

        #endregion
    }
}