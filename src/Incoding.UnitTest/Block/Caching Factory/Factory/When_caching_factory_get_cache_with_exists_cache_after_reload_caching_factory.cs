namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CachingFactory))]
    public class When_caching_factory_get_cache_with_exists_cache_after_reload_caching_factory
    {
        #region Static Fields

        protected static CachingFactory cachingFactory;

        #endregion

        #region Estabilish value

        static FakeSerializeObject objectFromCache;

        #endregion

        Establish establish = () =>
                                  {
                                      cachingFactory = new CachingFactory();
                                      var memoryListCachedProvider = new MemoryListCachedProvider();
                                      memoryListCachedProvider.Set(new FakeCacheKey(), new FakeSerializeObject { Name = Pleasure.Generator.TheSameString() });
                                      cachingFactory.Initialize(caching => caching.WithProvider(memoryListCachedProvider));
                                  };

        Because of = () => { objectFromCache = cachingFactory.Get<FakeSerializeObject>(new FakeCacheKey()); };

        It should_be_get_from_exists_cache = () => objectFromCache.Should(o =>
                                                                              {
                                                                                  o.ShouldNotBeNull();
                                                                                  o.Name.ShouldEqual(Pleasure.Generator.TheSameString());
                                                                              });
    }
}