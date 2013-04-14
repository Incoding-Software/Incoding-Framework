namespace Incoding.Data
{
    #region << Using >>

    using System.Linq;

    #endregion

    public interface IRepository
    {
        #region Methods

        /// <summary>
        ///     Persist the given entity instance
        /// </summary>
        /// <typeparam name="TEntity">Type entity</typeparam>
        /// <param name="entity">Entity instance</param>
        void Save<TEntity>(TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        ///     <see cref="Save{TEntity}" /> or update
        /// </summary>
        /// <typeparam name="TEntity">Type entity</typeparam>
        /// <param name="entity">Entity instance</param>
        void SaveOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        ///     Remove a entity from the datastore  by id
        /// </summary>
        /// <typeparam name="TEntity">Type entity</typeparam>
        /// <param name="id">Id</param>
        void Delete<TEntity>(object id) where TEntity : class, IEntity;

        /// <summary>
        ///     Remove a entity instance from the datastore
        /// </summary>
        /// <typeparam name="TEntity">Type entity</typeparam>
        /// <param name="entity">Persistence instance</param>
        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;

        TEntity GetById<TEntity>(object id) where TEntity : class, IEntity;

        TEntity LoadById<TEntity>(object id) where TEntity : class, IEntity;

        IQueryable<TEntity> Query<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null) where TEntity : class, IEntity;

        IncPaginatedResult<TEntity> Paginated<TEntity>(PaginatedSpecification paginatedSpecification, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null) where TEntity : class, IEntity;

        #endregion
    }
}