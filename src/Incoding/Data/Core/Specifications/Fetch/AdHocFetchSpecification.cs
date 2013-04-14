namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Incoding.Extensions;
    using JetBrains.Annotations;
    using NHibernate.Linq;

    #endregion

    public class AdHocFetchSpecification<TEntity>
    {
        #region Fields

        internal readonly List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> applies = new List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>();

        readonly List<Expression> expressions = new List<Expression>();

        #endregion

        #region Api Methods

        public AdHocFetchSpecification<TEntity> Join(Expression<Func<TEntity, object>> expression)
        {
            this.applies.Add(source => source.Fetch(expression));
            this.expressions.Add(expression);
            return this;
        }

        ////ncrunch: no coverage start
        [UsedImplicitly]
        public AdHocFetchSpecification<TEntity> Join<TChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, object>> thenFetch)
        {
            this.applies.Add(source => source.Fetch(expression).ThenFetch(thenFetch));
            this.expressions.AddRange(new List<Expression> { expression, thenFetch });
            return this;
        }

        [UsedImplicitly]
        public AdHocFetchSpecification<TEntity> Join<TChild, TNextChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, TNextChild>> thenFetch, Expression<Func<TNextChild, object>> nextThenChild)
        {
            this.applies.Add(source => source.Fetch(expression).ThenFetch(thenFetch).ThenFetch(nextThenChild));
            this.expressions.AddRange(new List<Expression> { expression, thenFetch, nextThenChild });
            return this;
        }

        [UsedImplicitly]
        public AdHocFetchSpecification<TEntity> Join<TChild, TNextChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, IEnumerable<TNextChild>>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch)
        {
            this.applies.Add(source => source.Fetch(expression).ThenFetchMany(thenFetch).ThenFetch(thenThenFetch));
            this.expressions.AddRange(new List<Expression> { expression, thenFetch, thenThenFetch });
            return this;
        }

        ////ncrunch: no coverage end
        public AdHocFetchSpecification<TEntity> JoinMany<TChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, object>> thenFetch)
        {
            this.applies.Add(source => source.FetchMany(fetch).ThenFetch(thenFetch));
            this.expressions.AddRange(new List<Expression> { fetch, thenFetch });
            return this;
        }

        ////ncrunch: no coverage start
        [UsedImplicitly]
        public AdHocFetchSpecification<TEntity> JoinMany<TChild, TNextChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, TNextChild>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch)
        {
            this.applies.Add(source => source.FetchMany(fetch).ThenFetch(thenFetch).ThenFetch(thenThenFetch));
            this.expressions.AddRange(new List<Expression> { fetch, thenFetch, thenThenFetch });
            return this;
        }

        [UsedImplicitly]
        public AdHocFetchSpecification<TEntity> JoinMany<TChild, TNextChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, IEnumerable<TNextChild>>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch)
        {
            this.applies.Add(source => source.FetchMany(fetch).ThenFetchMany(thenFetch).ThenFetch(thenThenFetch));
            this.expressions.AddRange(new List<Expression> { fetch, thenFetch, thenThenFetch });
            return this;
        }

        #endregion

        ////ncrunch: no coverage end
        #region Equals

        public override bool Equals(object obj)
        {
            return this.IsReferenceEquals(obj) && Equals(obj as AdHocFetchSpecification<TEntity>);
        }

        ////ncrunch: no coverage start
        public override int GetHashCode()
        {
            return 0;
        }

        ////ncrunch: no coverage end
        protected bool Equals(AdHocFetchSpecification<TEntity> other)
        {
            if (this.expressions.Count != other.expressions.Count)
            {
                Console.WriteLine(SpecificationMessageRes.AdHocFetchSpecification_Equal_diffrent_count_expressions.F(this.expressions.Count, other.expressions.Count));
                return false;
            }

            for (int i = 0; i < this.expressions.Count; i++)
            {
                if (!this.expressions[i].IsExpressionEqual(other.expressions[i]))
                {
                    Console.WriteLine(SpecificationMessageRes.AdHocFetchSpecification_Equal_diffrent_expressions.F(this.expressions[i], other.expressions[i]));
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}