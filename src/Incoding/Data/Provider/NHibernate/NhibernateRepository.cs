using NHibernate.Cfg;

namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Incoding.Extensions;
    using NHibernate;
    using NHibernate.Linq;
    using NHibernate.Persister.Entity;

    #endregion

    public class NhibernateRepository : IRepository
    {
        #region Fields

        Lazy<ISession> session;

        #endregion

        #region Constructors

        [ExcludeFromCodeCoverage]
        public NhibernateRepository(/*INhibernateSessionFactory sessionFactory*/)
        {
            //this.session = sessionFactory.GetCurrent();
        }

        #endregion
        
        #region IRepository Members

        public void ExecuteSql(string sql)
        {
            this.session.Value.CreateSQLQuery(sql).ExecuteUpdate();
        }

        public TProvider GetProvider<TProvider>() where TProvider : class
        {
            return session.Value as TProvider;
        }

        public void SetProvider(object provider)
        {
            session = (Lazy<ISession>) provider;
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            this.session.Value.Save(entity);
        }

        public void Saves<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity, new()
        {
            foreach (var entity in entities)
                Save(entity);
        }

        public void Flush()
        {
            this.session.Value.Flush();
        }

        public void SaveOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            this.session.Value.SaveOrUpdate(entity);
        }

        public void Delete<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            Delete(this.session.Value.Load<TEntity>(id));
        }

        public void DeleteByIds<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new()
        {
            var metadata = GetMetaData<TEntity>();
            string idColumnName = metadata.GetPropertyColumnNames("Id").FirstOrDefault();
            string tableName = metadata.TableName;
            string queryString = "DELETE FROM [{0}] WHERE {1} IN ({2})".F(tableName, idColumnName, ids.Select(o => o.GetType().IsAnyEquals(typeof(string), typeof(Guid)) ? "'{0}'".F(o.ToString()) : o.ToString()).AsString(","));
            this.session.Value
                .CreateSQLQuery(queryString)
                .ExecuteUpdate();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            this.session.Value.Delete(entity);
        }

        public void DeleteAll<TEntity>() where TEntity : class, IEntity, new()
        {
            this.session.Value.CreateSQLQuery("DELETE {0}".F(GetMetaData<TEntity>().TableName))
                .ExecuteUpdate();
        }

        public TEntity GetById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            if (id == null)
                return null;

            return this.session.Value.Get<TEntity>(id);
        }

        public TEntity LoadById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            if (id == null)
                return null;

            return this.session.Value.Load<TEntity>(id);
        }

        public IQueryable<TEntity> Query<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null) where TEntity : class, IEntity, new()
        {
            return this.session.Value.Query<TEntity>().Query(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
        }

        public IncPaginatedResult<TEntity> Paginated<TEntity>(PaginatedSpecification paginatedSpecification, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null) where TEntity : class, IEntity, new()
        {
            return this.session.Value.Query<TEntity>().Paginated(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
        }

        #endregion

        SingleTableEntityPersister GetMetaData<T>()
        {
            var metadata = this.session.Value.SessionFactory.GetClassMetadata(typeof(T));
            return (SingleTableEntityPersister)metadata;
        }
    }
}