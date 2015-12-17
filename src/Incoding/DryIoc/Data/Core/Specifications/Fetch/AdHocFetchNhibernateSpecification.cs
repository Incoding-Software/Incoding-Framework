namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using JetBrains.Annotations;
    using NHibernate.Linq;

    #endregion

    public class AdHocFetchNhibernateSpecification<TEntity> : AdHocFetchSpecificationBase<TEntity>
    {
        public override AdHocFetchSpecificationBase<TEntity> Join<TValue>(Expression<Func<TEntity, TValue>> expression)
        {
            this.applies.Add(source => source.Fetch(expression));
            this.expressions.Add(expression);
            return this;
        }

        [UsedImplicitly]
        public override AdHocFetchSpecificationBase<TEntity> Join<TChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, object>> thenFetch)
        {
            this.applies.Add(source => source.Fetch(expression).ThenFetch(thenFetch));
            this.expressions.AddRange(new List<Expression> { expression, thenFetch });
            return this;
        }

        [UsedImplicitly]
        public override AdHocFetchSpecificationBase<TEntity> Join<TChild, TNextChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, TNextChild>> thenFetch, Expression<Func<TNextChild, object>> nextThenChild)
        {
            this.applies.Add(source => source.Fetch(expression).ThenFetch(thenFetch).ThenFetch(nextThenChild));
            this.expressions.AddRange(new List<Expression> { expression, thenFetch, nextThenChild });
            return this;
        }

        [UsedImplicitly]
        public override AdHocFetchSpecificationBase<TEntity> Join<TChild, TNextChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, IEnumerable<TNextChild>>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch)
        {
            this.applies.Add(source => source.Fetch(expression).ThenFetchMany(thenFetch).ThenFetch(thenThenFetch));
            this.expressions.AddRange(new List<Expression> { expression, thenFetch, thenThenFetch });
            return this;
        }

        public override AdHocFetchSpecificationBase<TEntity> JoinMany<TChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, object>> thenFetch)
        {
            this.applies.Add(source => source.FetchMany(fetch).ThenFetch(thenFetch));
            this.expressions.AddRange(new List<Expression> { fetch, thenFetch });
            return this;
        }

        [UsedImplicitly]
        public override AdHocFetchSpecificationBase<TEntity> JoinMany<TChild, TNextChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, TNextChild>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch)
        {
            this.applies.Add(source => source.FetchMany(fetch).ThenFetch(thenFetch).ThenFetch(thenThenFetch));
            this.expressions.AddRange(new List<Expression> { fetch, thenFetch, thenThenFetch });
            return this;
        }

        [UsedImplicitly]
        public override AdHocFetchSpecificationBase<TEntity> JoinMany<TChild, TNextChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, IEnumerable<TNextChild>>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch)
        {
            this.applies.Add(source => source.FetchMany(fetch).ThenFetchMany(thenFetch).ThenFetch(thenThenFetch));
            this.expressions.AddRange(new List<Expression> { fetch, thenFetch, thenThenFetch });
            return this;
        }
    }
}