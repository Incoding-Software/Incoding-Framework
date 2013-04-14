namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Block.IoC;
    using Moq;

    #endregion

    public static class IoCFactoryTestEx
    {
        #region Factory constructors

        public static void Stub(this IoCFactory factory, Action<Mock<IIoCProvider>> action)
        {
            factory.Initialize(init => StubInit(init, action));
        }

        public static void StubResolve<TInstance>(this IoCFactory factory, Type type, TInstance mockInstance) where TInstance : class
        {
            Stub(factory, s => s.Setup(r => r.Get<TInstance>(type)).Returns(mockInstance));
        }

        public static void StubResolveAll<TInstance>(this IoCFactory factory, IEnumerable<TInstance> mockInstances)
        {
            Stub(factory, s => s.Setup(r => r.GetAll<TInstance>(typeof(TInstance))).Returns(mockInstances));
        }

        public static void StubTryResolve<TInstance>(this IoCFactory factory, TInstance mockInstance) where TInstance : class
        {
            Stub(factory, s => s.Setup(r => r.TryGet<TInstance>()).Returns(mockInstance));
        }

        public static void StubTryResolveByNamed<TInstance>(this IoCFactory factory, string named, TInstance mockInstance) where TInstance : class
        {
            Stub(factory, s => s.Setup(r => r.TryGetByNamed<TInstance>(named)).Returns(mockInstance));
        }

        #endregion

        static void StubInit(IoCInit init, Action<Mock<IIoCProvider>> action)
        {
            var mockProvider = Pleasure.Mock(action);
            if (init.Provider != null)
            {
                try
                {
                    mockProvider = Mock.Get(init.Provider);
                    action(mockProvider);
                }
                ////ncrunch: no coverage start
                catch (Exception)
                {
                    mockProvider = Pleasure.Mock(action);
                }
                ////ncrunch: no coverage end
            }

            init.WithProvider(mockProvider.Object);
        }
    }
}