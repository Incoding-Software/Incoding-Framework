namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(CachingFactory))]
    public class When_caching_factory_multi_thread_access_set_cached
    {
        #region Estabilish value

        protected static CachingFactory cachingFactory;

        static Mock<ICachedProvider> provider;

        #endregion

        Establish establish = () =>
                                  {
                                      provider = new Mock<ICachedProvider>();
                                      cachingFactory = new CachingFactory();
                                      cachingFactory.Initialize(caching => caching.WithProvider(provider.Object));
                                  };

        Because of = () => Pleasure.MultiThread.Do10(() => cachingFactory.Set(new FakeCacheKey(), new FakeSerializeObject()));

        It should_be_call_set_cached_only_at_once = () => provider.Verify(r => r.Set(Pleasure.MockIt.IsStrong(new FakeCacheKey()), Pleasure.MockIt.IsStrong(new FakeSerializeObject())), Times.Exactly(10));
    }
}