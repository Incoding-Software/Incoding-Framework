namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CachingFactoryTestEx))]
    public class When_caching_factor_test_ex_should_be_delete : Context_CachingFactoryTestEx
    {
        #region Establish value

        static FakeCache cacheKey;

        #endregion

        Establish establish = () =>
                                  {
                                      cacheKey = new FakeCache(Pleasure.Generator.String());
                                      CachingFactory.Instance.Stub();
                                  };

        Because of = () => CachingFactory.Instance.Delete(cacheKey);

        It should_be_delete = () => CachingFactory.Instance.ShouldBeDelete(cacheKey);
    }
}