namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Incoding.Extensions;

    #endregion

    public class EntityFrameworkRepository : IRepository
    {
        #region Fields

        readonly DbContext session;

        #endregion

        #region Constructors

        public EntityFrameworkRepository(DbContext session)
        {
            this.session = session;
        }

        public EntityFrameworkRepository() { }

        #endregion

        #region IRepository Members

        public void ExecuteSql(string sql)
        {
            session.Database.ExecuteSqlCommand(sql);
        }

        public TProvider GetProvider<TProvider>() where TProvider : class
        {
            return session as TProvider;
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            session.Set<TEntity>().Add(entity);
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
            if (session.Entry(entity).State == EntityState.Detached)
                session.Set<TEntity>().Add(entity);
        }

        public void Delete<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            Delete(GetById<TEntity>(id));
        }

        public void DeleteByIds<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new()
        {
            string idsAsString = ids.Select(o => o.GetType().IsAnyEquals(typeof(string), typeof(Guid)) ? "'{0}'".F(o.ToString()) : o.ToString()).AsString(",");
            string queryString = "DELETE FROM {0} WHERE {1} IN ({2})".F(session.GetTableName<TEntity>(), "Id", idsAsString);
            session.Database.ExecuteSqlCommand(queryString);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            session.Set<TEntity>().Remove(entity);
        }

        public void DeleteAll<TEntity>() where TEntity : class, IEntity, new()
        {
            session.Database.ExecuteSqlCommand("DELETE {0}".F(session.GetTableName<TEntity>()));
        }

        public TEntity GetById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            return session.Set<TEntity>().FirstOrDefault(r => r.Id == id);
        }

        public TEntity LoadById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            return session.Set<TEntity>().Find(id);
        }

        public IQueryable<TEntity> Query<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null) where TEntity : class, IEntity, new()
        {
            return session.Set<TEntity>().AsQueryable().Query(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
        }

        public IncPaginatedResult<TEntity> Paginated<TEntity>(PaginatedSpecification paginatedSpecification, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null) where TEntity : class, IEntity, new()
        {
            return session.Set<TEntity>().AsQueryable().Paginated(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
        }

        public void Clear() { }

        #endregion
    }
}