namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Configuration;
    using Incoding.Data;
    using Machine.Specifications;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using NCrunch.Framework;

    #endregion

    [Subject(typeof(MongoDbRepository)), Isolated]
    public class When_mongo_db_repository : Behavior_repository
    {
        Because of = () =>
                     {
                         BsonClassMap.RegisterClassMap<IncEntityBase>(map => map.UnmapProperty(r => r.Id));
                         var url = new MongoUrl(ConfigurationManager.ConnectionStrings["IncRealMongoDb"].ConnectionString);
                         var database = new MongoClient(url).GetServer().GetDatabase(url.DatabaseName);
                         repository = new MongoDbRepository(new MongoDatabaseDisposable(database));
                         repository.Init();
                     };

        Behaves_like<Behavior_repository> should_be_behavior;
    }
}