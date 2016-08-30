namespace Incoding.Block.Caching
{
    #region << Using >>

    using System;
    using Enyim.Caching;
    using Enyim.Caching.Configuration;
    using Enyim.Caching.Memcached;

    #endregion

    public sealed class MemCachedProvider : ICachedProvider
    {
        #region Fields

        readonly MemcachedClient memcached;

        #endregion

        #region Constructors

        public MemCachedProvider(string ip, int host)
                : this(ip, host, new DefaultTranscoder()) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemCachedProvider" /> class.
        /// </summary>
        public MemCachedProvider(string ip, int host, ITranscoder transcoder)
        {
            Guard.NotNullOrEmpty("ip", ip);

            /*Guard.IsConditional(() => host, host, (val) => val != 0);*/
            var config = new MemcachedClientConfiguration();
            config.AddServer(ip, host);
            config.Protocol = MemcachedProtocol.Text;

            config.SocketPool.ReceiveTimeout = new TimeSpan(0, 0, 10);
            config.SocketPool.ConnectionTimeout = new TimeSpan(0, 0, 10);
            config.SocketPool.DeadTimeout = new TimeSpan(0, 0, 20);

            config.Transcoder = transcoder;

            this.memcached = new MemcachedClient(config);
        }

        public MemCachedProvider(MemcachedClientConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);
            this.memcached = new MemcachedClient(configuration);
        }

        public MemCachedProvider(string sectionName)
        {
            Guard.NotNullOrWhiteSpace("sectionName", sectionName);
            this.memcached = new MemcachedClient(sectionName);
        }

        #endregion

        #region ICachedProvider Members

        public void Delete(ICacheKey key)
        {
            Guard.NotNull("key", key);
            this.memcached.Remove(key.GetName());
        }

        public void DeleteAll()
        {
            this.memcached.FlushAll();
        }

        public T Get<T>(ICacheKey key) where T : class
        {
            Guard.NotNull("key", key);
            return this.memcached.Get<T>(key.GetName());
        }

        public void Set<T>(ICacheKey key, T instance) where T : class
        {
            Guard.NotNull("key", key);
            this.memcached.Store(StoreMode.Set, key.GetName(), instance);
        }

        #endregion

        public void Dispose()
        {
            if(this.memcached != null)
                this.memcached.Dispose();
        }
    }
}