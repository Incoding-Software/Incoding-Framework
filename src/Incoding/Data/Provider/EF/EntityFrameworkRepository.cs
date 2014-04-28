namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Incoding.Extensions;

    #endregion

    public class EntityFrameworkRepository : IRepository
    {
        #region Fields

        readonly Lazy<DbContext> session;

        #endregion

        #region Constructors

        public EntityFrameworkRepository(IEntityFrameworkSessionFactory sessionFactory)
        {
            this.session = new Lazy<DbContext>(sessionFactory.GetCurrent);
        }

        #endregion

        #region IRepository Members

        public void ExecuteSql(string sql)
        {
            this.session.Value.Database.ExecuteSqlCommand(sql);
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            this.session.Value.Set<TEntity>().Add(entity);
        }

        public void Saves<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity, new()
        {
            foreach (var entity in entities)
                Save(entity);
        }

        public void Flush()
        {
            this.session.Value.SaveChanges();
        }

        public void SaveOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            if (this.session.Value.Entry(entity).State == EntityState.Detached)
                this.session.Value.Set<TEntity>().Add(entity);
        }

        public void Delete<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            Delete(GetById<TEntity>(id));
        }

        public void DeleteByIds<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new()
        {
            string idsAsString = ids.Select(o => o.GetType().IsAnyEquals(typeof(string), typeof(Guid)) ? "'{0}'".F(o.ToString()) : o.ToString()).AsString(",");
            string queryString = "DELETE FROM {0} WHERE {1} IN ({2})".F(this.session.Value.GetTableName<TEntity>(), "Id", idsAsString);
            this.session.Value.Database.ExecuteSqlCommand(queryString);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            this.session.Value.Set<TEntity>().Remove(entity);
        }

        public void DeleteAll<TEntity>() where TEntity : class, IEntity, new()
        {
            this.session.Value.Database.ExecuteSqlCommand("DELETE {0}".F(this.session.Value.GetTableName<TEntity>()));
        }

        public TEntity GetById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            return this.session.Value.Set<TEntity>().FirstOrDefault(r => r.Id == id);
        }

        public TEntity LoadById<TEntity>(object id) where TEntity : class, IEntity, new()
        {
            return this.session.Value.Set<TEntity>().Find(id);
        }

        public IQueryable<TEntity> Query<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null) where TEntity : class, IEntity, new()
        {
            return this.session.Value.Set<TEntity>().AsQueryable().Query(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
        }

        public Task<IQueryable<TEntity>> QueryAsync<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null) where TEntity : class, IEntity, new()
        {
            throw new NotImplementedException();
        }

        public IncPaginatedResult<TEntity> Paginated<TEntity>(PaginatedSpecification paginatedSpecification, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null) where TEntity : class, IEntity, new()
        {
            return this.session.Value.Set<TEntity>().AsQueryable().Paginated(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
        }

        #endregion
    }
}