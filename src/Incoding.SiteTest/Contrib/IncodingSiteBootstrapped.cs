namespace Incoding.SiteTest.Contrib
{
    #region << Using >>

    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentValidation;
    using FluentValidation.Mvc;
    using Incoding.Block;
    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using Incoding.MvcContrib.MVD;
    using NHibernate.Context;
    using NHibernate.Tool.hbm2ddl;
    using StructureMap.Graph;

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
            LoggingFactory.Instance.Initialize(logging =>
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                logging.WithPolicy(policy => policy.For(LogType.Trace).Use(FileLogger.WithAtOnceReplace(path, () => "Trace_{0}.txt".F(DateTime.Now.ToString("yyyyMMdd")))));
            });

            IoCFactory.Instance.Initialize(init => init.WithProvider(new StructureMapIoCProvider(registry =>
                                                                                                 {
                                                                                                     registry.For<IDispatcher>().Use<DefaultDispatcher>();
                                                                                                     registry.For<ITemplateFactory>().Use<TemplateDoTFactory>();

                                                                                                     var configure = Fluently
                                                                                                             .Configure()
                                                                                                             .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                                                                                                             .Database(MsSqlConfiguration.MsSql2012.ConnectionString(ConfigurationManager.ConnectionStrings["Main"].ConnectionString))
                                                                                                             .CurrentSessionContext<CallSessionContext>()
                                                                                                             .Mappings(configuration => configuration.FluentMappings
                                                                                                                                                     .Add<DelayToScheduler.Map>()
                                                                                                                                                     .AddFromAssembly(typeof(IncodingSiteBootstrapped).Assembly));

                                                                                                     registry.For<IManagerDataBase>().Singleton().Use(() => new NhibernateManagerDataBase(configure));
                                                                                                     registry.For<INhibernateSessionFactory>().Use(() => new NhibernateSessionFactory(configure));
                                                                                                     registry.For<IUnitOfWorkFactory>().Use<NhibernateUnitOfWorkFactory>();

                                                                                                     registry.Scan(r =>
                                                                                                                   {
                                                                                                                       r.TheCallingAssembly();
                                                                                                                       r.WithDefaultConventions();

                                                                                                                       r.ConnectImplementationsToTypesClosing(typeof(AbstractValidator<>));
                                                                                                                       r.AddAllTypesOf<ISetUp>();
                                                                                                                   });
                                                                                                 })));

            MVDExecute.SetInterception(() => new TraceMessageInterception());
            TemplateHandlebarsFactory.GetVersion =() => Guid.NewGuid().ToString();// disable cache template on server side as default
   

            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new IncValidatorFactory()));
            FluentValidationModelValidatorProvider.Configure();

            IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v3;

            IoCFactory.Instance.TryResolve<IDispatcher>().Push(new StartSchedulerCommand()
            {
                FetchSize = 10,
                Interval = 5.Seconds()
            });
        }

        #endregion
    }

    public class TraceMessageInterception : IMessageInterception
    {
        readonly Stopwatch time = new Stopwatch();

        public void OnBefore(IMessage message)
        {
            this.time.Start();
        }

        public void OnAfter(IMessage message)
        {
            this.time.Stop();
            var txt = "Message {0} execute at {1} milliseconds".F(message.GetType().Name, this.time.ElapsedMilliseconds);
            LoggingFactory.Instance.LogMessage(LogType.Trace, txt);
        }
    }
}