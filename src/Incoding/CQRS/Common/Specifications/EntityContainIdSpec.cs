namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Incoding.Data;

    #endregion

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

        #endregion

        public override Expression<Func<TEntity, bool>> IsSatisfiedBy()
        {
            return r => this.ids.Contains(r.Id);
        }
    }
}