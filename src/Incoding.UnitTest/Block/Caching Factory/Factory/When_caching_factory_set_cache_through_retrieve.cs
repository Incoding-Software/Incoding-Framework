namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(CachingFactory))]
    public class When_caching_factory_set_cache_through_retrieve
    {
        #region Estabilish value

        static FakeSerializeObject retrieve;

        static Mock<ISpy> spy;

        static FakeSerializeObject originalObject;

        static CachingFactory cachingFactory;

        #endregion

        Establish establish = () =>
                                  {
                                      cachingFactory = new CachingFactory();
                                      cachingFactory.Initialize(caching => caching
                                                                                   .WithProvider(new MemoryListCachedProvider()));

                                      originalObject = new FakeSerializeObject { Name = Pleasure.Generator.TheSameString() };
                                      cachingFactory.Retrieve(new FakeCacheKey(), () => originalObject);
                                      spy = Pleasure.Spy();
                                  };

        Because of = () =>
                         {
                             retrieve = cachingFactory.Retrieve<FakeSerializeObject>(new FakeCacheKey(), () =>
                                                                                                             {
                                                                                                                 spy.Object.Is();
                                                                                                                 return null;
                                                                                                             });
                         };

        It should_be_same_original_object = () => retrieve.ShouldEqualWeak(originalObject);

        It should_be_not_again_invoke = () => spy.Never();
    }
}