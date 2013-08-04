namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MemCachedProvider))]
    public class When_mem_cached_provider_init_with_quick_configuration_protocol_binary : Behaviors_cached_provider
    {
        Establish establish = () =>
                                  {
                                      cachedProvider = new MemCachedProvider("::1", 11211);
                                      cachedProvider.DeleteAll();
                                  };

        Behaves_like<Behaviors_cached_provider> should_be_verify;
    }
}