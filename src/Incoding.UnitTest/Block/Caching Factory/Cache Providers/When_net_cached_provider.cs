namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(NetCachedProvider))]
    public class When_net_cached_provider : Behaviors_cached_provider
    {
        Establish establish = () =>
                                  {
                                      cachedProvider = new NetCachedProvider();
                                      cachedProvider.DeleteAll();
                                  };

        Behaves_like<Behaviors_cached_provider> should_be_verify;
    }
}