namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(CachingFactory))]
    public class When_caching_factory_delete
    {
        #region Estabilish value

        static CachingFactory cachingFactory;

        #endregion

        Establish establish = () =>
                                  {
                                      cachingFactory = new CachingFactory();
                                      cachingFactory.Initialize(caching => caching
                                                                                   .RegistryPolicy(CachingPolicy.ForDeepDerived<ICacheKey>().NeverExpires())
                                                                                   .WithProvider(new MemoryListCachedProvider()));

                                      cachingFactory.Set(new FakeCacheKey(), new FakeSerializeObject { Name = typeof(FakeCacheKey).Name });
                                  };

        Because of = () => cachingFactory.Delete(new FakeCacheKey());

        It should_be_get_with_callback = () =>
                                             {
                                                 var spy = Pleasure.Mock<ISpy>();
                                                 Func<FakeSerializeObject> trigger = () =>
                                                                                         {
                                                                                             spy.Object.Is();
                                                                                             return new FakeSerializeObject();
                                                                                         };

                                                 cachingFactory.Retrieve(new FakeCacheKey(), trigger).ShouldNotBeNull();
                                                 spy.Once();
                                             };
    }
}