namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using Incoding.Data;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    [UsedImplicitly, ExcludeFromCodeCoverage]
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

        public EntityByIdSpec(Guid id)
        {
            this.id = id;
        }

        public EntityByIdSpec(int id)
        {
            this.id = id;
        }

        public EntityByIdSpec(long id)
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