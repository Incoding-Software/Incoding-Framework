namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IoCFactoryTestEx))]
    public class When_ioc_factory_stub_get_all : Context_IoC_Factory_TestEx
    {
        #region Estabilish value

        static ILogger fromIoC;

        #endregion

        Because of = () =>
                         {
                             IoCFactory.Instance.StubResolveAll(Pleasure.ToEnumerable(Pleasure.MockAsObject<ILogger>(), Pleasure.MockAsObject<ILogger>()));
                             IoCFactory.Instance.StubResolveAll(Pleasure.ToEnumerable(Pleasure.MockAsObject<IIoCProvider>(), Pleasure.MockAsObject<IIoCProvider>()));
                         };

        It should_be_get_all_logger = () => IoCFactory.Instance.ResolveAll<ILogger>().Count.ShouldEqual(2);

        It should_be_get_all_ioc_provider = () => IoCFactory.Instance.ResolveAll<IIoCProvider>().Count.ShouldEqual(2);
    }
}