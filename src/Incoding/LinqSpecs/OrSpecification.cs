namespace Incoding
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.ExpressionCombining;

    #endregion

    /// <summary>
    ///     The or specification.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    [Serializable]
    public class OrSpecification<T> : Specification<T>
    {
        #region Fields

        readonly Specification<T> spec1;

        readonly Specification<T> spec2;

        #endregion

        #region Constructors

        public OrSpecification(Specification<T> spec1, Specification<T> spec2)
        {
            this.spec1 = spec1;
            this.spec2 = spec2;
        }

        #endregion

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            var expr1 = this.spec1.IsSatisfiedBy();
            var expr2 = this.spec2.IsSatisfiedBy();

            if (expr1 == null && expr2 == null)
                return null;

            if (expr1 == null)
                return expr2;
            if (expr2 == null)
                return expr1;

            return expr1.OrElse(expr2);
        }
    }
}