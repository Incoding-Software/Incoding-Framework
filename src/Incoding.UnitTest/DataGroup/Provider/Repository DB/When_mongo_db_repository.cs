using System;

namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Configuration;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using NCrunch.Framework;

    #endregion

    [Subject(typeof(MongoDbRepository)), Isolated]
    public class When_mongo_db_repository : Behavior_repository
    {
        #region Establish value

        protected static IRepository GetRepository()
        {
            BsonClassMap.RegisterClassMap<IncEntityBase>(map => map.UnmapProperty(r => r.Id));
            var url = new MongoUrl(ConfigurationManager.ConnectionStrings["IncRealMongoDb"].ConnectionString);
            var database = new MongoClient(url).GetServer().GetDatabase(url.DatabaseName);
            var mongoDbRepository = new MongoDbRepository(/*Pleasure.MockStrictAsObject<IMongoDbSessionFactory>(mock => mock.Setup(r => r.GetCurrent()).Returns(new MongoDatabaseDisposable(database)))*/);
            mongoDbRepository.SetProvider(new Lazy<MongoDatabaseDisposable>(() => new MongoDatabaseDisposable(database)));
            return mongoDbRepository;
        }

        #endregion

        Because of = () =>
                         {
                             repository = GetRepository();
                             repository.Init();
                         };

        Behaves_like<Behavior_repository> should_be_behavior;
    }
}