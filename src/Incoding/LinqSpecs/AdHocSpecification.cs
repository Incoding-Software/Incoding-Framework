namespace Incoding
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Xml.Linq;
    using Incoding.ExpressionSerialization;

    #endregion

    [Serializable]
    public class AdHocSpecification<T> : Specification<T>
    {
        // private readonly Expression<Func<T, bool>> specification;
        #region Fields

        readonly string serializedExpressionXml;

        #endregion

        #region Constructors

        public AdHocSpecification(Expression<Func<T, bool>> specification)
        {
            if (specification == null)
                this.serializedExpressionXml = string.Empty;
            else
            {
                var cleanedExpression = ExpressionUtility.Ensure(specification);
                var serializer = new ExpressionSerializer();
                var serializedExpression = serializer.Serialize(cleanedExpression);
                this.serializedExpressionXml = serializedExpression.ToString();
            }
        }

        #endregion

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            if (string.IsNullOrWhiteSpace(this.serializedExpressionXml))
                return null;

            var serializer = new ExpressionSerializer();
            var serializedExpression = XElement.Parse(this.serializedExpressionXml);
            var specification = serializer.Deserialize<Func<T, bool>>(serializedExpression);
            return specification;
        }
    }
}