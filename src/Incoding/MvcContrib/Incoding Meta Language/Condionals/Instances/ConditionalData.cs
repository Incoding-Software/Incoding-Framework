namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class ConditionalData<TModel> : ConditionalBase
    {
        #region Fields

        string property;

        string method;

        object value;

        #endregion

        #region Constructors

        public ConditionalData(Expression<Func<TModel, bool>> expression, bool and)
                : base(ConditionalOfType.Data.ToString(), and)
        {
            var unary = expression.Body as UnaryExpression;
            if (unary != null)
            {
                this.inverse = true;
                Init(unary.Operand);
            }
            else
                Init(expression.Body);

            ////ncrunch: no coverage end
        }

        #endregion

        void Init(Expression body)
        {
            if (body is BinaryExpression)
                SetBinary(body as BinaryExpression);
            else if (body is MethodCallExpression)
                SetMethodCall(body as MethodCallExpression);
            else if (body.NodeType == ExpressionType.MemberAccess)
                SetMemberAccess(body as MemberExpression);
            else
                    ////ncrunch: no coverage start
                throw new ArgumentOutOfRangeException("body", "Not found logic for {0}".F(body.GetType().FullName));
        }

        void SetMemberAccess(MemberExpression memberExpression)
        {
            Set(memberExpression.Member.Name, true, "equal");
        }

        void Set(string propertyName, object value, string method)
        {
            this.property = propertyName;
            this.method = method;
            this.value = value.ReturnOrDefault(r => r.ToString(), string.Empty);
        }

        void SetMethodCall(MethodCallExpression expression)
        {
            string method = expression.Method.Name.ToLowerInvariant();

            switch (method)
            {
                case "contains":
                    var left = ((ConstantExpression)expression.Arguments[0]).Value;
                    string propContainsName = ((MemberExpression)expression.Object).Member.Name;
                    Set(propContainsName, left, method);
                    break;
                case "isempty":
                    string propIsEmptyName = (expression.Arguments[0] as MemberExpression).ReturnOrDefault(r => r.Member.Name, string.Empty);
                    Set(propIsEmptyName, null, method);
                    break;

                    ////ncrunch: no coverage start
                default:
                    throw new ArgumentOutOfRangeException("expression", "Can't found method {0}".F(method));

                    ////ncrunch: no coverage end
            }
        }

        void SetBinary(BinaryExpression expression)
        {
            string propertyName = string.Empty;
            string invokeValue = string.Empty;
            var left = expression.Left.NodeType == ExpressionType.Convert ? ((UnaryExpression)expression.Left).Operand : expression.Left;
            var right = expression.Right.NodeType == ExpressionType.Convert ? ((UnaryExpression)expression.Right).Operand : expression.Right;

            if (left.NodeType == ExpressionType.MemberAccess)
                propertyName = ((MemberExpression)left).Member.Name;
            else if (right.NodeType == ExpressionType.MemberAccess)
                propertyName = ((MemberExpression)right).Member.Name;

            invokeValue = left.NodeType.IsAnyEquals(ExpressionType.Constant, ExpressionType.Call)
                                  ? GetValue(left).ToString()
                                  : GetValue(right).ToString();

            Set(propertyName, invokeValue, expression.NodeType.ToStringLower());
        }

        object GetValue(Expression expression)
        {
            return expression.NodeType == ExpressionType.Convert
                           ? Expression.Lambda((expression as UnaryExpression).Operand).Compile().DynamicInvoke()
                           : Expression.Lambda(expression).Compile().DynamicInvoke();
        }

        public override object GetData()
        {
            return new
                       {
                               this.type,
                               this.property,
                               this.inverse,
                               this.method,
                               this.value,
                               this.and
                       };
        }
    }
}