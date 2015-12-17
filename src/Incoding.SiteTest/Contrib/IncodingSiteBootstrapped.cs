namespace Incoding.SiteTest.Contrib
{
    #region << Using >>

    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentValidation;
    using FluentValidation.Mvc;
    using Incoding.Block;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Maybe;
    using Incoding.MvcContrib;
    using NHibernate.Tool.hbm2ddl;
    using SimpleInjector;

    #endregion

    /// <summary>
    ///     Adds an unregistered type resolution for objects missing an IValidator.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    internal sealed class ValidateNothingDecorator<T> : AbstractValidator<T>
    {
        // I do nothing :-)
    }

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
            var container = new Container();
            container.Register<IDispatcher, DefaultDispatcher>();
            container.Register<ITemplateFactory, TemplateDoTFactory>();

            var configure = Fluently
                    .Configure()
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.ConnectionStrings["Main"].ConnectionString))
                    .Mappings(configuration => configuration.FluentMappings
                                                            .Add<DelayToScheduler.Map>()
                                                            .AddFromAssembly(typeof(IncodingSiteBootstrapped).Assembly));

            container.RegisterSingleton<INhibernateSessionFactory>(() => new NhibernateSessionFactory(configure));
            container.Register<IUnitOfWorkFactory, NhibernateUnitOfWorkFactory>();

            foreach (var implementingClass in typeof(AddProductCommand).Assembly.GetExportedTypes()
                                                                       .Where(type => type.BaseType.With(r => r.Name.StartsWith("AbstractValidator"))))
                container.Register(implementingClass.BaseType, implementingClass, Lifestyle.Singleton);

            // Add unregistered type resolution for objects missing an IValidator<T>
            // This should be placed after the registration of IValidator<>
            container.RegisterConditional(typeof(IValidator<>), typeof(ValidateNothingDecorator<>), Lifestyle.Singleton, context => !context.Handled);
            IoCFactory.Instance.Initialize(init => init.WithProvider(new SimpleInjectorIoCProvider(container)));

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