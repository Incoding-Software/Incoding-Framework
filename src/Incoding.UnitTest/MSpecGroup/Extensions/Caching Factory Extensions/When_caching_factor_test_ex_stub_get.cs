namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CachingFactoryTestEx))]
    public class When_caching_factor_test_ex_stub_get : Context_CachingFactoryTestEx
    {
        #region Estabilish value

        static FakeCache cacheKey;

        static string result;

        #endregion

        Establish establish = () =>
                                  {
                                      cacheKey = new FakeCache(Pleasure.Generator.String());
                                      CachingFactory.Instance.StubGet(cacheKey, Pleasure.Generator.TheSameString());
                                  };

        Because of = () => { result = CachingFactory.Instance.Get<string>(cacheKey); };

        It should_be_result = () => result.ShouldBeTheSameString();
    }
}