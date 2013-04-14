namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(CachingPolicy))]
    public class When_caching_policy_end_sliding
    {
        #region Estabilish value

        static CachingPolicy cachingPolicy;

        #endregion

        Because of = () => { cachingPolicy = CachingPolicy.For<FakeCacheKey>().EndSliding(Pleasure.NowPlush100Milliseconds); };

        It should_be_first_access_without_expires = () => cachingPolicy.IsExpires(new FakeCacheKey()).ShouldBeFalse();

        It should_be_after_each_access_sliding_time = () => Pleasure.DoFunc10(() =>
                                                                                  {
                                                                                      Pleasure.Sleep50Milliseconds();
                                                                                      return cachingPolicy.IsExpires(new FakeCacheKey());
                                                                                  }).ShouldBeFalse();
    }
}