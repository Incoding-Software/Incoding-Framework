namespace Incoding.Block.Caching
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Extensions;

    #endregion

    public sealed class MemoryListCachedProvider : ICachedProvider
    {
        #region Fields

        readonly Dictionary<string, object> cache;

        #endregion

        #region Constructors

        public MemoryListCachedProvider()
        {
            this.cache = new Dictionary<string, object>();
        }

        #endregion

        #region ICachedProvider Members

        public T Get<T>(ICacheKey key) where T : class
        {
            return this.cache.ContainsKey(key.GetName()) ? this.cache[key.GetName()] as T : null;
        }

        public void Set<T>(ICacheKey key, T instance) where T : class
        {
            this.cache.Set(key.GetName(), instance);
        }

        public void DeleteAll()
        {
            this.cache.Clear();
        }

        public void Delete(ICacheKey key)
        {
            if (this.cache.ContainsKey(key.GetName()))
                this.cache.Remove(key.GetName());
        }

        #endregion

        public void Dispose()
        {
            
        }
    }
}