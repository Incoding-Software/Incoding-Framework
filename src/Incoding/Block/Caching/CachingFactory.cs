namespace Incoding.Block.Caching
{
    #region << Using >>

    using System;
    using System.Collections.Concurrent;
    using Incoding.Block.Core;

    #endregion

    public sealed class CachingFactory : FactoryBase<CachingInit>
    {
        #region Static Fields

        static readonly ConcurrentDictionary<string, ICacheKey> cacheBuffer = new ConcurrentDictionary<string, ICacheKey>();

        static readonly object lockObject = new object();

        static volatile CachingFactory instance;

        #endregion

        #region Constructors

        public CachingFactory()
        {
            UnInitialize();
        }

        #endregion

        #region Properties

        public static CachingFactory Instance
        {
            ////ncrunch: no coverage start
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                            instance = new CachingFactory();
                    }
                }

                return instance;
            }

            ////ncrunch: no coverage end
        }

        #endregion

        #region Api Methods

        public void Delete(ICacheKey key)
        {
            this.init.Provider.Delete(key);
        }

        public void Set<TKey, TInstance>(TKey key, TInstance value)
                where TInstance : class, new()
                where TKey : class, ICacheKey
        {
            lock (cacheBuffer.GetOrAdd(key.GetName(), key))
                this.init.Provider.Set(key, value);
        }

        public TResult Get<TResult>(ICacheKey key) where TResult : class
        {
            lock (cacheBuffer.GetOrAdd(key.GetName(), key))
            {
                if (IsExpires(key))
                    Delete(key);

                return this.init.Provider.Get<TResult>(key);
            }
        }

        public TResult Retrieve<TResult>(ICacheKey key, Func<TResult> callback)
                where TResult : class, new()
        {
            var result = Get<TResult>(key);
            if (result != null)
                return result;

            result = callback();
            Set(key, result);
            return result;
        }

        #endregion

        bool IsExpires(ICacheKey key)
        {
            var cachingPolicy = this.init.GetLocalPolicy(key) ?? this.init.GetGlobalPolicy(key);
            return cachingPolicy != null && cachingPolicy.IsExpires(key);
        }

        public override void UnInitialize()
        {
            this.init = new CachingInit(new EmptyCachedProvider());
        }
    }
}