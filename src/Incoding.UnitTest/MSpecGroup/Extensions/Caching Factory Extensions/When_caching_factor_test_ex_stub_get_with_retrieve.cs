namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CachingFactoryTestEx))]
    public class When_caching_factor_test_ex_stub_get_with_retrieve : Context_CachingFactoryTestEx
    {
        #region Estabilish value

        static FakeCache cacheKey;

        static FakeSerializeObject result;

        #endregion

        Establish establish = () =>
                                  {
                                      cacheKey = new FakeCache(Pleasure.Generator.String());
                                      CachingFactory.Instance.StubGet(cacheKey, Pleasure.Generator.Invent<FakeSerializeObject>(dsl => dsl.Tuning(r => r.Name, Pleasure.Generator.TheSameString())));
                                  };

        Because of = () => { result = CachingFactory.Instance.Retrieve(cacheKey, () => Pleasure.Generator.Invent<FakeSerializeObject>()); };

        It should_be_result = () => result.Name.ShouldBeTheSameString();
    }
}