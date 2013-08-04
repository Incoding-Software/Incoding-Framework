namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MemoryListCachedProvider))]
    public class When_memory_list_cached_provider : Behaviors_cached_provider
    {
        Establish establish = () => { cachedProvider = new MemoryListCachedProvider(); };

        Behaves_like<Behaviors_cached_provider> should_be_verify_cached;
    }
}