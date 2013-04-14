namespace Incoding.Block.Caching
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public sealed class CachingInit
    {
        #region Fields

        readonly List<CachingPolicy> cachingGlobalPolicies = new List<CachingPolicy>();

        readonly Dictionary<int, CachingPolicy> cachingLocalPolicies = new Dictionary<int, CachingPolicy>();

        ICachedProvider cachedProvider;

        #endregion

        #region Constructors

        internal CachingInit(ICachedProvider cachedProvider)
        {
            WithProvider(cachedProvider);
        }

        #endregion

        #region Properties

        internal ICachedProvider CachedProvider
        {
            get { return this.Provider; }
        }

        public ICachedProvider Provider
        {
            get { return this.cachedProvider; }
        }

        #endregion

        #region Api Methods

        public CachingInit RegistryPolicy(CachingPolicy cachingPolicy)
        {
            Guard.NotNull("cachingPolicy", cachingPolicy);

            this.cachingGlobalPolicies.Add(cachingPolicy);
            return this;
        }

        public CachingInit WithProvider(ICachedProvider instance)
        {
            Guard.NotNull("instance", instance);

            this.cachedProvider = instance;
            return this;
        }

        #endregion

        internal CachingPolicy GetGlobalPolicy(ICacheKey key)
        {
            Guard.NotNull("key", key);
            return this.cachingGlobalPolicies.FirstOrDefault(policy => policy.IsSatisfied(key));
        }

        internal CachingPolicy GetLocalPolicy(ICacheKey key)
        {
            Guard.NotNull("key", key);
            if (!this.cachingLocalPolicies.ContainsKey(key.GetHashCode()))
                return null;

            var cachingPolicy = this.cachingLocalPolicies[key.GetHashCode()];
            return cachingPolicy;
        }
    }
}