namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(CachingFactory))]
    public class When_caching_factory_get_cache_by_filter_with_expires_always_as_local_policy
    {
        #region Estabilish value

        static string filterKey;

        static CachingFactory cachingFactory;

        #endregion

        Establish establish = () =>
                                  {
                                      cachingFactory = new CachingFactory();
                                      filterKey = "secretKey";
                                      cachingFactory.Initialize(caching => caching
                                                                                   .RegistryPolicy(CachingPolicy.ForAll().AlwaysExpires())
                                                                                   .WithProvider(new MemoryListCachedProvider()));
                                  };

        Because of = () => cachingFactory.Set(new FakeCacheKey(filterKey), Pleasure.Generator.Invent<FakeSerializeObject>());

        It should_be_global_policy = () =>
                                         {
                                             var spy = Pleasure.Mock<ISpy>();
                                             Pleasure.Do3((i) => cachingFactory.Retrieve(new FakeCacheKey(), () =>
                                                                                                                 {
                                                                                                                     spy.Object.Is();
                                                                                                                     return new FakeSerializeObject();
                                                                                                                 }).ShouldNotBeNull());
                                             spy.Exactly(3);
                                         };
    }
}