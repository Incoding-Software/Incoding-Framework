namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Incoding.Extensions;
    using Incoding.Quality;
    using JetBrains.Annotations;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;

    #endregion

    public class MongoDbRepository : IRepository
    {
        #region Fields

        readonly MongoDatabase database;

        #endregion

        #region Constructors

        public MongoDbRepository(IMongoDbSessionFactory sessionFactory)
        {
            this.database = sessionFactory.GetCurrent().Instance;
        }

        #endregion

        #region IRepository Members

        [UsedImplicitly, Obsolete(ObsoleteMessage.NotSupportForThisImplement, true), ExcludeFromCodeCoverage]
        public void ExecuteSql(string sql)
        {
            throw new NotImplementedException();
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            GetCollection<TEntity>().Insert(entity);
        }

        public void Saves<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity, new()
        {
            GetCollection<TEntity>().InsertBatch(entities);
        }

        [UsedImplicitly, Obsolete(ObsoleteMessage.NotSupportForThisImplement, true), ExcludeFromCodeCoverage]
        public void Flush() { }

        public void SaveOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            var id = entity.TryGetValue("Id");
            if (GetById<TEntity>(id) == null)
            {
                Save(entity);
                return;
            }

            var update = new UpdateBuilder();
            foreach (var property in typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                    .Where(r => !r.Name.EqualsWithInvariant("Id")))
            {
                var value = property.GetValue(entity, null);

                BsonValue bsonValue = BsonNull.Value;
                if (value != null)
                {
                    var type = (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                       ? property.PropertyType.GetGenericArguments()[0]
                                       : property.PropertyType;

                    if (type == typeof(string))
                        bsonValue = new BsonString(value.ToString());
                    else if (type == typeof(bool))
                        bsonValue = new BsonBoolean((bool)value);
                    else if (type == typeof(DateTime))
                        bsonValue = new BsonDateTime((DateTime)value);
                    else if (type == typeof(long))
                        bsonValue = new BsonInt64((long)value);
                    else if (type == typeof(int))
                        bsonValue = new BsonInt32((int)value);
                    else if (type == typeof(byte[]))
                        bsonValue = new BsonBinaryData((byte[])value);
                    else if (type == typeof(Guid))
                        bsonValue = new BsonBinaryData((Guid)value);
                    else if (type.IsEnum)
                        bsonValue = new BsonString(value.ToString());
                    else if (type.IsImplement<IEnumerable>())
                        bsonValue = new BsonArray((IEnumerable)value);
                    else if (type.IsClass && type.IsImplement<IEntity>())
                        bsonValue = new BsonDocumentWrapper(value);
                    else
                        throw new ArgumentOutOfRangeException("propertyType {0} does not bson value".F(type));
                }

                update.Set(property.Name, bsonValue);
            }

            GetCollection<TEntity>().Update(MongoDB.Driver.Builders.Query<TEntity>.EQ(r => r.Id, id), update);
        }

        public void Delete<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            var query = MongoDB.Driver.Builders.Query<TEntity>.EQ(r => r.Id, id);
            GetCollection<TEntity>().Remove(query);
        }

        public void DeleteByIds<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new()
        {
            var query = MongoDB.Driver.Builders.Query<TEntity>.In(r => r.Id, ids);
            GetCollection<TEntity>().Remove(query);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            Delete<TEntity>(entity.TryGetValue("Id"));
        }

        public void DeleteAll<TEntity>() where TEntity : class, IEntity, new()
        {
            GetCollection<TEntity>().RemoveAll();
        }

        public TEntity GetById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            if (id == null)
                return null;
            var query = MongoDB.Driver.Builders.Query<TEntity>.EQ(r => r.Id, id);
            return GetCollection<TEntity>().FindOne(query);
        }

        public TEntity LoadById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            return GetById<TEntity>(id);
        }

        public IQueryable<TEntity> Query<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null) where TEntity : class, IEntity, new()
        {
            return GetCollection<TEntity>()
                    .AsQueryable<TEntity>()
                    .Query(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
        }

        public Task<IQueryable<TEntity>> QueryAsync<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null) where TEntity : class, IEntity, new()
        {
            throw new NotImplementedException();
        }

        public IncPaginatedResult<TEntity> Paginated<TEntity>(PaginatedSpecification paginatedSpecification, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null) where TEntity : class, IEntity, new()
        {
            return GetCollection<TEntity>()
                    .AsQueryable<TEntity>()
                    .Paginated(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
        }

        #endregion

        MongoCollection<TEntity> GetCollection<TEntity>()
        {
            return this.database.GetCollection<TEntity>(typeof(TEntity).Name);
        }
    }
}