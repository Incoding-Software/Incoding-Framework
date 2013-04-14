namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(CachingPolicy))]
    public class When_caching_policy_end_absolute
    {
        #region Estabilish value

        static CachingPolicy cachingPolicy;

        #endregion

        Because of = () => { cachingPolicy = CachingPolicy.For<FakeCacheKey>().EndAbsolute(Pleasure.NowPlush100Milliseconds); };

        It should_be_in_time_cache = () => cachingPolicy.IsExpires(new FakeCacheKey()).ShouldBeFalse();

        It should_be_not_in_time = () => Pleasure.Do3((i) =>
                                                          {
                                                              Pleasure.Sleep100Milliseconds();
                                                              cachingPolicy.IsExpires(new FakeCacheKey()).ShouldBeTrue();
                                                          });
    }
}