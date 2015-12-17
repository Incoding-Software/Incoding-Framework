namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using Incoding.Extensions;
    using JetBrains.Annotations;
    using Raven.Client.Linq;

    #endregion

    public class AdHocFetchRavenDbSpecification<TEntity> : AdHocFetchSpecificationBase<TEntity>
    {
        public override AdHocFetchSpecificationBase<TEntity> Join<TValue>(Expression<Func<TEntity, TValue>> expression)
        {
            this.applies.Add(source => ((IRavenQueryable<TEntity>)source).Customize(x => x.Include<TEntity, TValue>(expression.ToBox())));
            this.expressions.Add(expression);
            return this;
        }

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public override AdHocFetchSpecificationBase<TEntity> Join<TChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, object>> thenFetch)
        {
            throw new NotSupportedException();
        }

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public override AdHocFetchSpecificationBase<TEntity> Join<TChild, TNextChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, TNextChild>> thenFetch, Expression<Func<TNextChild, object>> nextThenChild)
        {
            throw new NotSupportedException();
        }

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public override AdHocFetchSpecificationBase<TEntity> Join<TChild, TNextChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, IEnumerable<TNextChild>>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch)
        {
            throw new NotSupportedException();
        }

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public override AdHocFetchSpecificationBase<TEntity> JoinMany<TChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, object>> thenFetch)
        {
            throw new NotSupportedException();
        }

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public override AdHocFetchSpecificationBase<TEntity> JoinMany<TChild, TNextChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, TNextChild>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch)
        {
            throw new NotSupportedException();
        }

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public override AdHocFetchSpecificationBase<TEntity> JoinMany<TChild, TNextChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, IEnumerable<TNextChild>>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch)
        {
            throw new NotSupportedException();
        }
    }
}