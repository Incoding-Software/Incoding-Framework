namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.Extensions;
    using Incoding.Utilities;
    using Machine.Specifications;
    using Ninject.Extensions.Conventions;
    using Ninject.Modules;

    #endregion

    [Subject(typeof(NinjectIoCProvider))]
    public class When_ninject_created_by_modules : Context_IoC_Provider
    {
        #region Fake classes

        class CustomInstrumentationModule : NinjectModule
        {
            #region Override

            public override void Load()
            {
                Bind<IEmailSender>().ToConstant(defaultInstance);
            }

            #endregion
        }

        class CustomLoggerModule : NinjectModule
        {
            #region Override

            public override void Load()
            {
                Bind<ILogger>().To<ConsoleLogger>().Named(consoleNameInstance);
            }

            #endregion
        }

        class CustomEmailSenderModule : NinjectModule
        {
            public override void Load()
            {
                Kernel.Bind(scanner => scanner.From(typeof(IFakePlugIn).Assembly)
                                              .Select(type => type.IsImplement<IFakePlugIn>() && !type.IsAnyEquals(typeof(IFakePlugIn)))
                                              .BindAllInterfaces());
            }
        }

        #endregion

        Establish establish = () => { ioCProvider = new NinjectIoCProvider(new CustomInstrumentationModule(), new CustomLoggerModule(), new CustomEmailSenderModule()); };

        Behaves_like<Behaviors_get_ioc_provider> verify_get_and_try_get_operation;

        Behaves_like<Behaviors_expected_exception_ioc_provider> verify_expected_exception;

        Behaves_like<Behaviors_forward_ioc_provider> verify_forward;

        Behaves_like<Behaviors_eject_ioc_provider> verify_eject;
    }
}