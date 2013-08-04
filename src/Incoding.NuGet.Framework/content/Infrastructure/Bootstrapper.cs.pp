namespace $rootnamespace$
{

    using System.Configuration;
    using System.Linq;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentValidation;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.EventBroker;
    using NHibernate.Context;


    public static class Bootstrapper
    {
        #region Factory constructors

        public static void Start()
        {
            IoCFactory.Instance.Initialize(init => init.WithProvider(new StructureMapIoCProvider(registry =>
                                                                                                     {
                                                                                                         registry.For<IDispatcher>().Use<DefaultDispatcher>();
                                                                                                         registry.For<IEventBroker>().Use<DefaultEventBroker>();

                                                                                                         var configure = Fluently
                                                                                                                 .Configure()
                                                                                                                 .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.ConnectionStrings["Main"].ConnectionString))
                                                                                                                 .Mappings(configuration => configuration.FluentMappings.AddFromAssembly(typeof(Bootstrapper).Assembly))
                                                                                                                 .CurrentSessionContext<ThreadStaticSessionContext>();

                                                                                                         registry.For<IManagerDataBase>().Use(new NhibernateManagerDataBase(configure));
                                                                                                         registry.For<INhibernateSessionFactory>().Singleton().Use(() => new NhibernateSessionFactory(configure));

                                                                                                         registry.For<IUnitOfWorkFactory>().Use<NhibernateUnitOfWorkFactory>();
                                                                                                         registry.For<IUnitOfWork>().Use<NhibernateUnitOfWork>();
                                                                                                         registry.For<IRepository>().Use<NhibernateRepository>();

                                                                                                         registry.Scan(r =>
                                                                                                                           {
                                                                                                                               r.WithDefaultConventions();
                                                                                                                               r.TheCallingAssembly();
                                                                                                                               
                                                                                                                               r.ConnectImplementationsToTypesClosing(typeof(AbstractValidator<>));
                                                                                                                               r.ConnectImplementationsToTypesClosing(typeof(IEventSubscriber<>));                                                                                                                               
                                                                                                                               r.AddAllTypesOf<ISetUp>();
                                                                                                                           });
                                                                                                     })));

            foreach (var setUp in IoCFactory.Instance.ResolveAll<ISetUp>().OrderBy(r => r.GetOrder()))
                setUp.Execute();
        }

        #endregion
    }


}