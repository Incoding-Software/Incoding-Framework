namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Linq;
    using Incoding.Extensions;
    using NHibernate;
    using NHibernate.Linq;

    #endregion

    public class NhibernateRepository : IRepository
    {
        #region Fields

        readonly Lazy<ISession> session;

        #endregion

        #region Constructors

        public NhibernateRepository(INhibernateSessionFactory sessionFactory)
        {
            Guard.NotNull("sessionFactory", sessionFactory);
            this.session = new Lazy<ISession>(sessionFactory.GetCurrentSession);
        }

        #endregion

        #region IRepository Members

        public void Save<TEntity>(TEntity entity) where TEntity : class, IEntity
        {            
            this.session.Value.Save(entity);
        }

        public void Flush()
        {
            this.session.Value.Flush();
        }

        public void SaveOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity
        {         
            this.session.Value.SaveOrUpdate(entity);
        }

        public void Delete<TEntity>(object id) where TEntity : class, IEntity
        {
            var entity = LoadById<TEntity>(id);
            this.session.Value.Delete(entity);
        }

        public TEntity GetById<TEntity>(object id) where TEntity : class, IEntity
        {
            if (id == null)
                return null;

            return this.session.Value.Get<TEntity>(id);
        }

        public TEntity LoadById<TEntity>(object id) where TEntity : class, IEntity
        {
            if (id == null)
                return null;

            return this.session.Value.Load<TEntity>(id);
        }

        public IQueryable<TEntity> Query<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null) where TEntity : class, IEntity
        {
            return GetQuery(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification);
        }

        public IncPaginatedResult<TEntity> Paginated<TEntity>(PaginatedSpecification paginatedSpecification, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null) where TEntity : class, IEntity
        {
            int totalCount = GetQuery(null, whereSpecification, null, null).Count();
            var paginatedItems = GetQuery(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification).ToList();
            return new IncPaginatedResult<TEntity>(paginatedItems, totalCount);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            this.session.Value.Delete(entity);
        }

        #endregion

        IQueryable<TEntity> GetQuery<TEntity>(OrderSpecification<TEntity> orderSpecification, Specification<TEntity> whereSpecification, FetchSpecification<TEntity> fetchSpecification, PaginatedSpecification paginatedSpecification) where TEntity : class, IEntity
        {
            var source = this.session.Value.Query<TEntity>();

            if (whereSpecification != null && whereSpecification.IsSatisfiedBy() != null)
                source = source.Where(whereSpecification.IsSatisfiedBy());

            if (orderSpecification != null)
            {
                var order = new AdHocOrderSpecification<TEntity>();
                orderSpecification.SortedBy()(order);
                source = order.applies.Aggregate(source, (current, apply) => apply(current));
            }

            if (paginatedSpecification != null)
                source = source.Page(paginatedSpecification.CurrentPage, paginatedSpecification.PageSize);

            if (fetchSpecification != null)
            {
                var fetch = new AdHocFetchSpecification<TEntity>();
                fetchSpecification.FetchedBy()(fetch);
                source = fetch.applies.Aggregate(source, (current, apply) => apply(current));
            }

            return source;
        }
    }
}