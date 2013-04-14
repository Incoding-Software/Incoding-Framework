namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.Data;

    #endregion

    public class EntityByIdSpec<TEntity> : Specification<TEntity> where TEntity : IEntity
    {
        #region Fields

        readonly object id;

        #endregion

        #region Constructors

        public EntityByIdSpec(string id)
        {
            this.id = id;
        }

        #endregion

        public override Expression<Func<TEntity, bool>> IsSatisfiedBy()
        {
            return r => r.Id == this.id;
        }
    }
}