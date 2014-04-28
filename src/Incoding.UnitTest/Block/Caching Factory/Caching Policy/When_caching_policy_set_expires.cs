namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CachingPolicy))]
    public class When_caching_policy_set_expires
    {
        #region Establish value

        static CachingPolicy cachingPolicy;

        #endregion

        Because of = () =>
                         {
                             cachingPolicy = CachingPolicy.For<FakeCacheKey>().SetExpires(key =>
                                                                                              {
                                                                                                  var fake = key as FakeCacheKey;
                                                                                                  return fake == null || fake.IsReadyToExpires;
                                                                                              });
                         };

        It should_be_not_expires = () => cachingPolicy.IsExpires(new FakeCacheKey { IsReadyToExpires = false }).ShouldBeFalse();

        It should_be_expires_by_conditional = () => cachingPolicy.IsExpires(new FakeCacheKey { IsReadyToExpires = true }).ShouldBeTrue();
    }
}