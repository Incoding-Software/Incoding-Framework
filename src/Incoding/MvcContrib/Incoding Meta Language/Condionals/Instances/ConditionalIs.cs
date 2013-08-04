namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.Extensions;

    #endregion

    public class ConditionalIs : ConditionalEval
    {
        #region Constructors

        public ConditionalIs(Expression<Func<bool>> expression, bool and)
                : base(string.Empty, and)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    if (expression.Body is MethodCallExpression)
                        SetMethodCall(expression.Body as MethodCallExpression);
                    else if (expression.Body is BinaryExpression)
                        SetBinary(expression.Body as BinaryExpression);
                    else if (expression.Body.Type == typeof(Boolean))
                        SetBoolean(expression.Body);
                    break;

                    ////ncrunch: no coverage start
                default:
                    throw new ArgumentOutOfRangeException("expression", "Not found logic for {0}".F(expression.NodeType));

                    ////ncrunch: no coverage end
            }
        }

        #endregion

        void SetBoolean(Expression expression)
        {
            var value = Selector.Value(Expression.Lambda(expression).Compile().DynamicInvoke());
            Set(value, value, ExpressionType.Equal.ToStringLower());
        }

        void SetMethodCall(MethodCallExpression expression)
        {
            var originalValue = Expression.Lambda(expression.Arguments[0]).Compile().DynamicInvoke();

            string method = expression.Method.Name.ToLowerInvariant();
            switch (method)
            {
                case "contains":
                    var containsValue = Expression.Lambda(expression.Arguments[1]).Compile().DynamicInvoke();
                    Set(originalValue, containsValue, method);
                    break;
                case "isempty":
                    Set(originalValue, string.Empty, method);
                    break;

                    ////ncrunch: no coverage start
                default:
                    throw new ArgumentOutOfRangeException("expression", "Can't found method {0}".F(method));

                    ////ncrunch: no coverage end
            }
        }

        void Set(object left, object right, string method)
        {
            this.code = JavaScriptCodeTemplate.Conditional_Value.F(left.ToString(), right.ToString(), method);
        }

        void SetBinary(BinaryExpression expression)
        {
            var left = Expression.Lambda(expression.Left).Compile().DynamicInvoke();
            if (!(left is Selector) && expression.Left.NodeType != ExpressionType.Convert)
                left = Selector.Value(left);

            var right = Expression.Lambda(expression.Right).Compile().DynamicInvoke();
            if (!(right is Selector) && expression.Right.NodeType != ExpressionType.Convert)
                right = Selector.Value(right);

            Set(left, right, expression.NodeType.ToStringLower());
        }
    }
}