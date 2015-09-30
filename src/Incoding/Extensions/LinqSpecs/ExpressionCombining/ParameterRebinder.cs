namespace Incoding.ExpressionCombining
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq.Expressions;

    #endregion

    // ------------------------------------------------------------------------------------------
    // This code was taken from the MSDN Blog meek: LINQ to Entities: Combining Predicates
    // http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx
    // ------------------------------------------------------------------------------------------
    ////ncrunch: no coverage start
    public class ParameterRebinder : ExpressionVisitor
    {
        #region Fields

        readonly Dictionary<ParameterExpression, ParameterExpression> map;

        #endregion

        #region Constructors

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        #endregion

        #region Factory constructors

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        #endregion

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (this.map.TryGetValue(p, out replacement))
                p = replacement;
            return base.VisitParameter(p);
        }
    }

    ////ncrunch: no coverage end
}