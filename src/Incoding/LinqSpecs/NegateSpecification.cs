namespace Incoding
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;

    #endregion

    [Serializable]
    public class NegateSpecification<T> : Specification<T>
    {
        #region Fields

        readonly Specification<T> spec;

        #endregion

        #region Constructors

        public NegateSpecification(Specification<T> spec)
        {
            this.spec = spec;
        }

        #endregion

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            var isSatisfiedBy = this.spec.IsSatisfiedBy();
            return Expression.Lambda<Func<T, bool>>(Expression.Not(isSatisfiedBy.Body), isSatisfiedBy.Parameters);
        }
    }
}