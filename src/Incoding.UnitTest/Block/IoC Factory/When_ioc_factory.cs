namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(IoCFactory))]
    public class When_ioc_factory
    {
        #region Estabilish value

        protected static Mock<IIoCProvider> iocProvider;

        static IoCFactory iocFactory;

        #endregion

        Establish establish = () =>
                                  {
                                      iocProvider = new Mock<IIoCProvider>();
                                      iocFactory = new IoCFactory();
                                  };

        Because of = () => iocFactory.Initialize(init => init.WithProvider(iocProvider.Object));

        It should_be_get_by_type = () =>
                                       {
                                           iocProvider.Setup(r => r.Get<ILogger>(typeof(ILogger))).Returns(new ClipboardLogger());
                                           iocFactory.Resolve<ILogger>(typeof(ILogger)).ShouldBeOfType<ClipboardLogger>();
                                       };

        It should_be_try_get_by_type = () =>
                                           {
                                               iocProvider.Setup(r => r.TryGet<ILogger>(typeof(ILogger))).Returns(new ClipboardLogger());
                                               iocFactory.TryResolve<ILogger>(typeof(ILogger)).ShouldBeOfType<ClipboardLogger>();
                                           };

        It should_be_try_resolve = () =>
                                       {
                                           iocProvider.Setup(r => r.TryGet<ILogger>(typeof(ILogger))).Returns((ILogger)null);
                                           iocFactory.TryResolve<ILogger>().ShouldBeNull();
                                       };

        It should_be_resolve_all = () =>
                                       {
                                           iocProvider.Setup(r => r.GetAll<ILogger>(typeof(ILogger))).Returns(new List<ILogger> { new ClipboardLogger(), new ClipboardLogger() });
                                           iocFactory.ResolveAll<ILogger>().Count.ShouldEqual(2);
                                       };

        It should_be_try_get_by_named = () =>
                                            {
                                                iocProvider.Setup(r => r.TryGetByNamed<ILogger>(Pleasure.Generator.TheSameString())).Returns(new ClipboardLogger());
                                                iocFactory.TryResolveByNamed<ILogger>(Pleasure.Generator.TheSameString()).ShouldNotBeNull();
                                            };

        It should_be_try_get_without_registry_type = () => iocFactory.TryResolve<IFake>().ShouldBeNull();

        It should_be_forward = () =>
                                   {
                                       iocFactory.Forward(new ConsoleLogger());
                                       iocProvider.Verify(r => r.Forward(Pleasure.MockIt.IsAny<ConsoleLogger>()));
                                   };

        It should_be_eject = () =>
                                 {
                                     iocFactory.Eject<ILogger>();
                                     iocProvider.Verify(r => r.Eject<ILogger>());
                                 };
    }
}