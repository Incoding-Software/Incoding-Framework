namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CachingFactoryTestEx))]
    public class When_caching_factor_test_ex_should_be_set : Context_CachingFactoryTestEx
    {
        #region Fake classes

        class FakeInstance
        {
            #region Properties

            public string Id { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        static FakeCache cacheKey;

        static FakeInstance instance;

        #endregion

        Establish establish = () =>
                                  {
                                      cacheKey = new FakeCache(Pleasure.Generator.String());
                                      instance = new FakeInstance
                                                     {
                                                             Id = Pleasure.Generator.TheSameString()
                                                     };
                                      CachingFactory.Instance.Stub();
                                  };

        Because of = () => CachingFactory.Instance.Set(cacheKey, instance);

        It should_be_set = () => CachingFactory.Instance.ShouldBeSet(cacheKey, instance);
    }
}