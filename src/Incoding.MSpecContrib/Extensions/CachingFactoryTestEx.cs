namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using Incoding.Block.Caching;
    using Incoding.Extensions;
    using Machine.Specifications;
    using Moq;

    #endregion

    public static class CachingFactoryTestEx
    {
        #region Factory constructors

        public static void ShouldBeDelete<TCacheKey>(this CachingFactory factory, TCacheKey cacheKey) where TCacheKey : ICacheKey
        {
            var provider = ((CachingInit)factory.TryGetValue("init")).Provider;
            Mock.Get(provider).Verify(r => r.Delete(Pleasure.MockIt.Is<TCacheKey>(key => key.GetName().ShouldEqual(cacheKey.GetName()))));
        }

        public static void ShouldBeGet<TInstance, TCacheKey>(this CachingFactory factory, TCacheKey cacheKey)
                where TCacheKey : ICacheKey
                where TInstance : class, new()
        {
            var provider = ((CachingInit)factory.TryGetValue("init")).Provider;
            Mock.Get(provider).Verify(r => r.Get<TInstance>(Pleasure.MockIt.Is<TCacheKey>(key => key.GetName().ShouldEqual(cacheKey.GetName()))));
        }

        public static void ShouldBeSet<TCacheKey, TInstance>(this CachingFactory factory, TCacheKey cacheKey, TInstance instance)
                where TInstance : class
                where TCacheKey : ICacheKey
        {
            var provider = ((CachingInit)factory.TryGetValue("init")).Provider;
            Mock.Get(provider).Verify(r => r.Set(Pleasure.MockIt.Is<TCacheKey>(key => key.GetName().ShouldEqual(cacheKey.GetName())), Pleasure.MockIt.IsStrong(instance)));
        }

        public static void Stub(this CachingFactory factory, Action<Mock<ICachedProvider>> action)
        {
            factory.Initialize(init => StubInit(init, action));
        }

        public static void Stub(this CachingFactory factory)
        {
            factory.Initialize(init => StubInit(init, mock => { }));
        }

        public static void StubGet<TCacheKey, TInstance>(this CachingFactory factory, TCacheKey cacheKey, TInstance instance)
                where TInstance : class
                where TCacheKey : ICacheKey
        {
            factory.Stub(mock => mock.Setup(r => r.Get<TInstance>(Pleasure.MockIt.Is<TCacheKey>(key => key.GetName().ShouldEqual(cacheKey.GetName())))).Returns(instance));
        }

        #endregion

        static void StubInit(CachingInit cachingInit, Action<Mock<ICachedProvider>> action)
        {
            var mockProvider = Pleasure.Mock(action);
            if (cachingInit.Provider != null)
            {
                try
                {
                    mockProvider = Mock.Get(cachingInit.Provider);
                    action(mockProvider);
                }  
                        ////ncrunch: no coverage start
                catch (Exception)
                {
                    mockProvider = Pleasure.Mock(action);
                }

                ////ncrunch: no coverage end
            }

            cachingInit.WithProvider(mockProvider.Object);
        }
    }
}