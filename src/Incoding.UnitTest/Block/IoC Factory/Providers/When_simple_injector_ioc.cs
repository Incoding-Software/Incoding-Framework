namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using FluentValidation;
    using Incoding.Block.IoC;
    using Machine.Specifications;
    using SimpleInjector;

    #endregion

    [Subject(typeof(SimpleInjectorIoCProvider))]
    public class When_simple_injector_ioc : Context_IoC_Provider
    {
        Establish establish = () =>
                              {
                                  var container = new Container();
                                  container.Register(() => defaultInstance);
                                  //container.Register<ILogger, ClipboardLogger>();
                                  //container.Register<ILogger, ConsoleLogger>();
                                  container.RegisterCollection(typeof(IFakePlugIn),new[] { typeof(FakePlugIn1), typeof(FakePlugIn2) });
                                  container.Register(typeof(AbstractValidator<FakeCommand>), typeof(TestValidator));
                                                                    
                                  ioCProvider = new SimpleInjectorIoCProvider(container);

                                  
                              };

        It should_be_spec = () => { ioCProvider.TryGet<IValidator>(typeof(AbstractValidator<FakeCommand>)).ShouldNotBeNull(); };

        Behaves_like<Behaviors_disposable_ioc_provider> verify_disposable;

        Behaves_like<Behaviors_eject_ioc_provider> verify_eject;

        Behaves_like<Behaviors_expected_exception_ioc_provider> verify_expected_exception;

        Behaves_like<Behaviors_forward_ioc_provider> verify_forward;

        Behaves_like<Behaviors_get_ioc_provider> verify_get_and_try_get_operation;

        public class FakeCommand
        {
            public string Name { get; set; }
        }

        public class TestValidator : AbstractValidator<FakeCommand>
        {
            public TestValidator()
            {
                RuleFor(r => r.Name).NotEmpty();
            }
        }
    }
}