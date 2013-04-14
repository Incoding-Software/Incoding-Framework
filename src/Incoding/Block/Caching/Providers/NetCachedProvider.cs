namespace Incoding.Block.Caching
{
    #region << Using >>

    using System.Collections;
    using System.Web;

    #endregion

    public sealed class NetCachedProvider : ICachedProvider
    {
        #region ICachedProvider Members

        public T Get<T>(ICacheKey key) where T : class
        {
            return HttpRuntime.Cache.Get(key.GetName()) as T;
        }

        public void Set<T>(ICacheKey key, T instance) where T : class
        {
            HttpRuntime.Cache.Insert(key.GetName(), instance);
        }

        public void DeleteAll()
        {
            foreach (DictionaryEntry cacheEntry in HttpRuntime.Cache)
                HttpRuntime.Cache.Remove(cacheEntry.Key.ToString());
        }

        public void Delete(ICacheKey key)
        {
            HttpRuntime.Cache.Remove(key.GetName());
        }

        #endregion
    }
}