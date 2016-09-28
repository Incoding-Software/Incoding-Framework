namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections.Concurrent;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;

    #endregion

    public class MongoDbSessionFactory : IMongoDbSessionFactory
    {
        #region Static Fields

        static readonly ConcurrentDictionary<string, MongoServer> servers = new ConcurrentDictionary<string, MongoServer>();

        [ThreadStatic]
        static MongoDatabaseDisposable currentSession;

        #endregion

        #region Fields

        readonly string defaultUrl;

        #endregion

        #region Constructors

        static MongoDbSessionFactory()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(IncEntityBase)))
                BsonClassMap.RegisterClassMap<IncEntityBase>(map => map.UnmapProperty(r => r.Id));
        }

        public MongoDbSessionFactory(string defaultUrl)
        {
            this.defaultUrl = defaultUrl;
            servers.AddOrUpdate(defaultUrl, s => new MongoClient(new MongoUrl(defaultUrl)).GetServer(), (s, server) => server);
        }

        #endregion

        #region IMongoDbSessionFactory Members
        
        public MongoDatabaseDisposable Open(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                connectionString = this.defaultUrl;

            var url = new MongoUrl(connectionString);
            var server = servers.GetOrAdd(connectionString, s => new MongoClient(url).GetServer());
            currentSession = new MongoDatabaseDisposable(server.GetDatabase(url.DatabaseName));
            return currentSession;
        }

        #endregion

    }
}