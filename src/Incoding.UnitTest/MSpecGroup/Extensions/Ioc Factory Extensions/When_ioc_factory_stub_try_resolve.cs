namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Incoding.Utilities;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IoCFactoryTestEx))]
    public class When_ioc_factory_stub_try_resolve : Context_IoC_Factory_TestEx
    {
        #region Establish value

        static ILogger fromIoC;

        #endregion

        Because of = () =>
                         {
                             IoCFactory.Instance.StubTryResolve(Pleasure.MockAsObject<ILogger>());
                             IoCFactory.Instance.StubTryResolve(Pleasure.MockAsObject<IEmailSender>());
                         };

        It should_be_resolve_logger = () => IoCFactory.Instance.TryResolve<ILogger>().ShouldNotBeNull();

        It should_be_resolve_email = () => IoCFactory.Instance.TryResolve<IEmailSender>().ShouldNotBeNull();
    }
}