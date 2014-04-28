namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using Incoding.Data;
    using JetBrains.Annotations;

    #endregion

    [UsedImplicitly, ExcludeFromCodeCoverage]
    public class EntityContainIdSpec<TEntity> : Specification<TEntity> where TEntity : IEntity
    {
        #region Fields

        readonly List<object> ids;

        #endregion

        #region Constructors

        public EntityContainIdSpec(string[] ids)
        {
            this.ids = ids.OfType<object>().ToList();
        }

        public EntityContainIdSpec(Guid[] ids)
        {
            this.ids = ids.OfType<object>().ToList();
        }

        public EntityContainIdSpec(int[] ids)
        {
            this.ids = ids.OfType<object>().ToList();
        }

        public EntityContainIdSpec(long[] ids)
        {
            this.ids = ids.OfType<object>().ToList();
        }

        #endregion

        public override Expression<Func<TEntity, bool>> IsSatisfiedBy()
        {
            return r => this.ids.Contains(r.Id);
        }
    }
}