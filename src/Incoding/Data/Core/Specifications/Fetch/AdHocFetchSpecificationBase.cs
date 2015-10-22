namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Incoding.Extensions;

    #endregion

    public abstract class AdHocFetchSpecificationBase<TEntity>
    {
        #region Fields

        internal readonly List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> applies = new List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>();

        protected readonly List<Expression> expressions = new List<Expression>();

        #endregion

        #region Api Methods

        public abstract AdHocFetchSpecificationBase<TEntity> Join<TValue>(Expression<Func<TEntity, TValue>> expression);

        public abstract AdHocFetchSpecificationBase<TEntity> Join<TChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, object>> thenFetch);

        public abstract AdHocFetchSpecificationBase<TEntity> Join<TChild, TNextChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, TNextChild>> thenFetch, Expression<Func<TNextChild, object>> nextThenChild);

        public abstract AdHocFetchSpecificationBase<TEntity> Join<TChild, TNextChild>(Expression<Func<TEntity, TChild>> expression, Expression<Func<TChild, IEnumerable<TNextChild>>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch);

        public abstract AdHocFetchSpecificationBase<TEntity> JoinMany<TChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, object>> thenFetch);

        public abstract AdHocFetchSpecificationBase<TEntity> JoinMany<TChild, TNextChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, TNextChild>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch);

        public abstract AdHocFetchSpecificationBase<TEntity> JoinMany<TChild, TNextChild>(Expression<Func<TEntity, IEnumerable<TChild>>> fetch, Expression<Func<TChild, IEnumerable<TNextChild>>> thenFetch, Expression<Func<TNextChild, object>> thenThenFetch);

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            return this.IsReferenceEquals(obj) && Equals(obj as AdHocFetchNhibernateSpecification<TEntity>);
        }

        ////ncrunch: no coverage start
        public override int GetHashCode()
        {
            return 0;
        }

        ////ncrunch: no coverage end
        /// 
        protected bool Equals(AdHocFetchNhibernateSpecification<TEntity> other)
        {
            if (this.expressions.Count != other.expressions.Count)
            {
                Console.WriteLine(Resources.AdHocFetchSpecification_Equal_diffrent_count_expressions.F(this.expressions.Count, other.expressions.Count));
                return false;
            }

            for (int i = 0; i < this.expressions.Count; i++)
            {
                if (!this.expressions[i].IsExpressionEqual(other.expressions[i]))
                {
                    Console.WriteLine(Resources.AdHocFetchSpecification_Equal_diffrent_expressions.F(this.expressions[i], other.expressions[i]));
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}