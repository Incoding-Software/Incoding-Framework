namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(CachingFactory)), Ignore("Need fixed")]
    public class When_caching_factory_multi_thread_access_delete_cached
    {
        #region Estabilish value

        protected static CachingFactory cachingFactory;

        static Mock<ICachedProvider> provider;

        #endregion

        Establish establish = () =>
                                  {
                                      provider = Pleasure.Mock<ICachedProvider>();
                                      cachingFactory = new CachingFactory();
                                      cachingFactory.Initialize(caching => caching.WithProvider(provider.Object));
                                  };

        Because of = () => Pleasure.MultiThread.Do10(() => cachingFactory.Delete(new FakeCacheKey()));

        It should_be_call_set_cached_only_at_once = () => provider.Verify(r => r.Delete(Pleasure.MockIt.IsStrong(new FakeCacheKey())), Times.Exactly(1));
    }
}