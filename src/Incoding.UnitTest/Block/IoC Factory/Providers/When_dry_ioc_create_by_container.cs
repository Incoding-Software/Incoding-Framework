namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using DryIoc;
    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DryIocProvider))]
    public class When_dry_ioc_create_by_container : Context_IoC_Provider
    {
        Establish establish = () =>
                              {
                                  var container = new Container(rules:Rules.Default,scopeContext:new ThreadScopeContext());
                                  container.RegisterInstance(defaultInstance);
                                  container.Register<ClipboardLogger>();
                                  container.Register<ConsoleLogger>(serviceKey: consoleNameInstance);
                                  ioCProvider = new DryIocProvider(container);
                              };

        //Behaves_like<Behaviors_disposable_ioc_provider> verify_disposable;

        Behaves_like<Behaviors_eject_ioc_provider> verify_eject;

        Behaves_like<Behaviors_expected_exception_ioc_provider> verify_expected_exception;

        Behaves_like<Behaviors_forward_ioc_provider> verify_forward;

        Behaves_like<Behaviors_get_ioc_provider> verify_get_and_try_get_operation;
    }
}