namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class ConditionalIs : ConditionalBase
    {
        readonly bool fakeMode;

        void SetUnary(UnaryExpression expression)
        {
            var operand = (expression.Operand is UnaryExpression)
                                  ? ((UnaryExpression)expression.Operand).Operand
                                  : expression.Operand;
            if (IsMethodCall(operand))
            {
                this.inverse = expression.NodeType == ExpressionType.Not;
                SetMethodCall(operand as MethodCallExpression);
            }
            else
            {
                var valueOperand = Expression.Lambda(operand).Compile().DynamicInvoke();
                Set(valueOperand, expression.NodeType != ExpressionType.Not, ExpressionType.Equal.ToStringLower());
            }
        }

        void SetBoolean(Expression expression)
        {
            var value = fakeMode ? Selector.Result.For(((MemberExpression)expression).Member.Name)
                                : Expression.Lambda(expression).Compile().DynamicInvoke();
            Set(true, value, ExpressionType.Equal.ToStringLower());
        }

        void SetMethodCall(MethodCallExpression expression)
        {
            string currentMethod = expression.Method.Name.ToLowerInvariant();
            var originalValue = Expression.Lambda(expression.Arguments[0]).Compile().DynamicInvoke();

            switch (currentMethod)
            {
                case IsMethod.isContains:
                    var containsValue = Expression.Lambda(expression.Arguments[1]).Compile().DynamicInvoke();
                    Set(originalValue, containsValue, currentMethod);
                    break;
                case IsMethod.isEmpty:
                    Set(originalValue, string.Empty, currentMethod);
                    break;
                default:
                    var compileValue = Expression.Lambda(expression).Compile().DynamicInvoke();
                    Set(true, compileValue, "equal");
                    break;
            }
        }

        void Set(object l, object r, string m)
        {
            this.left = l is Selector ? l.With(s => s.ToString()).Recovery(string.Empty) : new ValueSelector(l);
            this.right = r is Selector ? r.With(s => s.ToString()).Recovery(string.Empty) : new ValueSelector(r);
            this.method = m;
        }

        void SetBinary(BinaryExpression expression)
        {
            if (IsMethodCall(expression.Left))
            {
                this.inverse = !(bool)Expression.Lambda(expression.Right).Compile().DynamicInvoke();
                SetMethodCall(expression.Left as MethodCallExpression);
                return;
            }

            Set(GetValue(expression.Left, this.fakeMode), GetValue(expression.Right), expression.NodeType.ToStringLower());
        }

        object GetValue(Expression expression, bool setMode = false)
        {
            if (setMode)
                return Selector.Result.For(((MemberExpression)expression).Member.Name);

            return expression.NodeType == ExpressionType.Convert
                           ? Expression.Lambda(((UnaryExpression)expression).Operand).Compile().DynamicInvoke()
                           : Expression.Lambda(expression).Compile().DynamicInvoke();
        }

        bool IsMethodCall(Expression expression)
        {
            if (expression is MethodCallExpression)
            {
                var callExpression = expression as MethodCallExpression;
                return callExpression.Method.Name.IsAnyEqualsIgnoreCase(IsMethod.isContains, IsMethod.isEmpty);
            }

            return false;
        }

        public override object GetData()
        {
            return new
                   {
                           this.type,
                           this.inverse,
                           this.left,
                           this.right,
                           this.method,
                           this.and
                   };
        }

        #region Nested classes

        class IsMethod
        {
            #region Constants

            public const string isContains = "iscontains";

            public const string isEmpty = "isempty";

            #endregion
        }

        #endregion

        #region Fields

        string left;

        string right;

        object method;

        #endregion

        #region Constructors

        public ConditionalIs(Expression body, bool and, bool fakeMode = false)
                : base(ConditionalOfType.Is.ToString(), and)
        {
            this.fakeMode = fakeMode;
            if (body is MethodCallExpression)
                SetMethodCall(body as MethodCallExpression);
            else if (body is UnaryExpression)
                SetUnary(body as UnaryExpression);
            else if (body is BinaryExpression)
                SetBinary(body as BinaryExpression);
            else if (body.Type == typeof(Boolean))
                SetBoolean(body);
            else
            ////ncrunch: no coverage start
                throw new ArgumentOutOfRangeException("expression", "Not found logic for {0}".F(body.GetType()));
            ////ncrunch: no coverage end
        }

        public ConditionalIs(Expression<Func<bool>> expression, bool and)
                : this(expression.Body, and) { }

        #endregion
    }
}