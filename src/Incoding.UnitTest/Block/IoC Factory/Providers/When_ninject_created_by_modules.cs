namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using FluentValidation;
    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.Extensions;
    using Incoding.Utilities;
    using Machine.Specifications;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Modules;

    #endregion

    [Subject(typeof(NinjectIoCProvider))]
    public class When_ninject_created_by_modules : Context_IoC_Provider
    {
        Establish establish = () => { ioCProvider = new NinjectIoCProvider(new StandardKernel(new CustomEmailSenderModule(), new CustomInstrumentationModule(), new CustomLoggerModule())); };

        Behaves_like<Behaviors_disposable_ioc_provider> verify_disposable;

        Behaves_like<Behaviors_eject_ioc_provider> verify_eject;

        Behaves_like<Behaviors_expected_exception_ioc_provider> verify_expected_exception;

        Behaves_like<Behaviors_forward_ioc_provider> verify_forward;

        Behaves_like<Behaviors_get_ioc_provider> verify_get_and_try_get_operation;

        #region Fake classes

        class CustomInstrumentationModule : NinjectModule
        {
            #region Override

            public override void Load()
            {
                Bind<IEmailSender>().ToConstant(defaultInstance);
                Bind(typeof(AbstractValidator<FakeCommand>)).To<TestValidator>();
            }

            #endregion
        }

        class CustomLoggerModule : NinjectModule
        {
            #region Override

            public override void Load()
            {
                Bind<ILogger>().To<ConsoleLogger>().Named(consoleNameInstance.ToString());
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
    }
}