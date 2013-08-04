namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IoCFactoryTestEx))]
    public class When_ioc_factory_stub_try_resolve_by_named : Context_IoC_Factory_TestEx
    {
        #region Estabilish value

        static ILogger fromIoC;

        #endregion

        Because of = () => IoCFactory.Instance.StubTryResolveByNamed(Pleasure.Generator.TheSameString(), Pleasure.MockAsObject<ILogger>());

        It should_be_resolve = () => IoCFactory.Instance.TryResolveByNamed<ILogger>(Pleasure.Generator.TheSameString()).ShouldNotBeNull();

        It should_be_not_resolve = () => IoCFactory.Instance.TryResolveByNamed<ILogger>(Pleasure.Generator.String()).ShouldBeNull();
    }
}