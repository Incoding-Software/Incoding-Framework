namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Behaviors]
    public class Behaviors_cached_provider
    {
        #region Estabilish value

        protected static FakeSerializeObject valueToCache = Pleasure.Generator.Invent<FakeSerializeObject>();

        protected static ICachedProvider cachedProvider;

        #endregion

        It should_be_found_in_cached = () =>
                                           {
                                               cachedProvider.Set(new FakeCacheKey(), valueToCache);
                                               Pleasure.Do10((i) => cachedProvider.Get<FakeSerializeObject>(new FakeCacheKey()).ShouldNotBeNull());
                                           };

        It should_be_delete_all = () =>
                                      {
                                          cachedProvider.Set(new FakeCacheKey(), valueToCache);
                                          cachedProvider.Set(new FakeCacheCustomHierarchy(), new FakeSerializeObject());

                                          cachedProvider.DeleteAll();

                                          cachedProvider.Get<FakeSerializeObject>(new FakeCacheKey()).ShouldBeNull();
                                          cachedProvider.Get<FakeSerializeObject>(new FakeCacheCustomHierarchy()).ShouldBeNull();
                                      };

        It should_be_delete = () =>
                                  {
                                      cachedProvider.Set(new FakeCacheKey(), valueToCache);
                                      cachedProvider.Delete(new FakeCacheKey());

                                      cachedProvider.Get<FakeSerializeObject>(new FakeCacheKey()).ShouldBeNull();
                                  };
    }
}