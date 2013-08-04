namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CachingFactory))]
    public class When_caching_factory_get_cache_by_deep_derived_with_expires_always_as_global_policy
    {
        #region Estabilish value

        static CachingFactory cachingFactory;

        #endregion

        Establish establish = () =>
                                  {
                                      cachingFactory = new CachingFactory();
                                      cachingFactory.Initialize(caching =>
                                                                caching
                                                                        .WithPolicy(r => r.ForDeepDerived<ICacheKey>().NeverExpires())
                                                                        .WithProvider(new MemoryListCachedProvider()));
                                  };

        Because of = () => cachingFactory.Set(new FakeCacheKey(), Pleasure.Generator.Invent<FakeSerializeObject>());

        It should_be_while_try_get_and_not_usage_trigger = () => Pleasure.Do3((i) =>
                                                                                  {
                                                                                      var spy = Pleasure.Mock<ISpy>();
                                                                                      cachingFactory.Retrieve(new FakeCacheKey(), () =>
                                                                                                                                      {
                                                                                                                                          spy.Object.Is();
                                                                                                                                          return Pleasure.Generator.Invent<FakeSerializeObject>();
                                                                                                                                      }).ShouldNotBeNull();

                                                                                      spy.Never();
                                                                                  });
    }
}