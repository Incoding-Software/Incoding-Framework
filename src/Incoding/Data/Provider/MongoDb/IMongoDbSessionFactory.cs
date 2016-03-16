namespace Incoding.Data
{
    using MongoDB.Driver;

    public interface IMongoDbSessionFactory : ISessionFactory<MongoDatabaseDisposable> 
    { }
}