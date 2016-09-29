namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Incoding.Extensions;
    using Incoding.Quality;
    using JetBrains.Annotations;
    using Raven.Abstractions.Data;
    using Raven.Client;
    using Raven.Client.Linq;
    using Raven.Imports.Newtonsoft.Json;

    #endregion

    public class RavenDbRepository : IRepository
    {
        #region Static Fields

        static readonly MethodInfo save = typeof(RavenDbRepository).GetMethod("Save");

        static readonly MethodInfo saves = typeof(RavenDbRepository).GetMethod("Saves");

        static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> cache = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();

        #endregion

        #region Fields

        readonly IDocumentSession session;
        
        #endregion

        #region Constructors

        public RavenDbRepository(IDocumentSession session)
        {
            this.session = session;
        }

        public RavenDbRepository() { }

        #endregion

        #region IRepository Members

        [UsedImplicitly, Obsolete(ObsoleteMessage.NotSupportForThisImplement, true), ExcludeFromCodeCoverage]
        public void ExecuteSql(string sql)
        {
            throw new NotImplementedException();
        }

        public TProvider GetProvider<TProvider>() where TProvider : class
        {
            return session as TProvider;
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            if (session.Advanced.HasChanged(entity))
                return;

            session.Store(entity);
            foreach (var propertyInfo in cache.GetOrAdd(typeof(TEntity), type => type.GetProperties()
                                                                                     .Where(r => r.HasAttribute<JsonIgnoreAttribute>() &&
                                                                                                 r.CanRead && r.CanWrite)))
            {
                var value = propertyInfo.GetValue(entity, null);
                if (value == null)
                    continue;

                bool isEnumerableValue = value is IEnumerable;
                var entityType = isEnumerableValue ? propertyInfo.PropertyType.GetGenericArguments()[0] : propertyInfo.PropertyType;

                (isEnumerableValue ? saves : save)
                        .MakeGenericMethod(entityType)
                        .Invoke(this, new[] { value });
            }
        }

        public void Saves<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity, new()
        {
            foreach (var entity in entities)
                Save(entity);
        }

        public void Flush()
        {
            session.SaveChanges();
        }

        public void SaveOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            session.Store(entity);
        }

        public void Delete<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            Delete(LoadById<TEntity>(id));
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            session.Delete(entity);
        }

        public void DeleteAll<TEntity>() where TEntity : class, IEntity, new()
        {
            session.Advanced.DocumentStore.DatabaseCommands.DeleteByIndex("Raven/DocumentsByEntityName", 
                                                                          new IndexQuery
                                                                          {
                                                                                  Query = "Tag:" + session.Advanced.DocumentStore.Conventions.GetTypeTagName(typeof(TEntity)), 
                                                                          }, 
                                                                          allowStale: true);
        }

        public TEntity GetById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            if (id == null)
                return null;

            return id is string
                           ? session.Load<TEntity>(id.ToString())
                           : session.Load<TEntity>((ValueType)id);
        }

        public TEntity LoadById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            return GetById<TEntity>(id);
        }

        public IQueryable<TEntity> Query<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null) where TEntity : class, IEntity, new()
        {
            return GetRavenQueryable<TEntity>()
                    .Query(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
        }

        public IncPaginatedResult<TEntity> Paginated<TEntity>(PaginatedSpecification paginatedSpecification, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null) where TEntity : class, IEntity, new()
        {
            return GetRavenQueryable<TEntity>()
                    .Paginated(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void DeleteByIds<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new()
        {
            foreach (var id in ids)
                Delete<TEntity>(id);
        }

        #endregion

        IRavenQueryable<TEntity> GetRavenQueryable<TEntity>()
        {
            var mapIndex = session.Advanced.DocumentStore.DatabaseCommands.GetIndex("Map" + typeof(TEntity).Name);
            bool hasMap = mapIndex != null;
            var genericTypes = hasMap ? new[] { typeof(TEntity), mapIndex.GetType() } : new[] { typeof(TEntity) };

            var queryMethod = typeof(IDocumentSession).GetMethods()
                                                      .First(r => r.Name.EqualsWithInvariant("Query") &&
                                                                  r.GetGenericArguments().Count() == genericTypes.Length &&
                                                                  !r.GetParameters().Any())
                                                      .MakeGenericMethod(genericTypes);

            return queryMethod.Invoke(session, new object[] { }) as IRavenQueryable<TEntity>;
        }
    }
}