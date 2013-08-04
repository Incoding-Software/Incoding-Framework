namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CachingFactory))]
    public class When_caching_factory_retrieve_cache_by_hierarchy_with_expires_end_absolute_as_global_policy
    {
        #region Estabilish value

        static CachingFactory cachingFactory;

        #endregion

        Because of = () =>
                         {
                             cachingFactory = new CachingFactory();
                             cachingFactory.Initialize(caching => caching
                                                                          .WithPolicy(r => r.ForFirstDerived<FakeCacheCustomHierarchy>().EndAbsolute(Pleasure.NowPlush100Milliseconds))
                                                                          .WithProvider(new MemoryListCachedProvider()));
                             cachingFactory.Set(new FakeCacheCustomHierarchyDerived(), Pleasure.Generator.Invent<FakeSerializeObject>());
                         };

        It should_be_get_with_callback = () =>
                                             {
                                                 var spy = Pleasure.Mock<ISpy>();
                                                 Pleasure.Sleep1000Milliseconds();
                                                 cachingFactory.Retrieve(new FakeCacheCustomHierarchyDerived(), () =>
                                                                                                                    {
                                                                                                                        spy.Object.Is();
                                                                                                                        return new FakeSerializeObject();
                                                                                                                    }).ShouldNotBeNull();
                                                 spy.Once();
                                             };
    }
}