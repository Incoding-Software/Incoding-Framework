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
            if (expression.Body is BinaryExpression)
                SetBinary(expression.Body as BinaryExpression);
            else if (expression.Body is MethodCallExpression)
                SetMethodCall(expression.Body as MethodCallExpression);
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
                SetMemberAccess(expression.Body as MemberExpression);
            else                                                                                                                                                                                                        
                    ////ncrunch: no coverage start
                throw new ArgumentOutOfRangeException("expression", "Not found logic for {0}".F(expression.Body.GetType().FullName));

            ////ncrunch: no coverage end
        }

        #endregion

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
            var left = Expression.Lambda(expression.Right).Compile().DynamicInvoke();
            string propertyName = ((MemberExpression)expression.Left).Member.Name;
            Set(propertyName, left, expression.NodeType.ToStringLower());
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