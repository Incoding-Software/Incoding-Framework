namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;

    #endregion

    public class FakeCacheCustomHierarchy : ICacheKey
    {
        #region ICacheKey Members

        public string GetName()
        {
            return GetType().Name;
        }

        #endregion
    }
}