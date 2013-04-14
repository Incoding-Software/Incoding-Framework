namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.Extensions;
    using Incoding.Utilities;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using Ninject.Extensions.Conventions;

    #endregion

    [Subject(typeof(NinjectIoCProvider))]
    public class When_ninject_created_by_in_line_initialize : Context_IoC_Provider
    {
        Establish establish = () =>
                                  {
                                      ioCProvider = new NinjectIoCProvider(kernel =>
                                                                               {
                                                                                   kernel.Bind<IEmailSender>().ToConstant(defaultInstance);
                                                                                   kernel.Bind<ILogger>().To<ConsoleLogger>().Named(consoleNameInstance);
                                                                                   kernel.Bind(scanner => scanner.From(typeof(IFakePlugIn).Assembly)
                                                                                                                 .Select(type => type.IsImplement<IFakePlugIn>() && !type.IsAnyEquals(typeof(IFakePlugIn)))
                                                                                                                 .BindAllInterfaces());
                                                                               });
                                  };

        Behaves_like<Behaviors_get_ioc_provider> verify_get_and_try_get_operation;

        Behaves_like<Behaviors_expected_exception_ioc_provider> verify_expected_exception;

        Behaves_like<Behaviors_forward_ioc_provider> verify_forward;

        Behaves_like<Behaviors_eject_ioc_provider> verify_eject;
    }
}