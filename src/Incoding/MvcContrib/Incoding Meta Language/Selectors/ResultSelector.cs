namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.Extensions;

    #endregion

    public class ResultSelector : Selector
    {
        #region Constructors

        internal ResultSelector()
                : this(string.Empty) { }

        internal ResultSelector(string prop)
                : base("||result*{0}||".F(prop)) { }

        #endregion

        #region Api Methods

        public ResultSelector For<T>(Expression<Func<T, object>> property)
        {
            var isMethod = property.Body is MethodCallExpression;
            if (isMethod)
            {
                var body = (MethodCallExpression)property.Body;
                var isSelect = body.Method.Name.IsAnyEqualsIgnoreCase("Select");
                if (isSelect)
                {
                    MemberExpression left = (MemberExpression)body.Arguments[0];
                    LambdaExpression right = (LambdaExpression)body.Arguments[1];
                    return new ResultSelector("{0}.{1}({2})".F(left.Member.Name, body.Method.Name, ((MemberExpression)right.Body).Member.Name));
                }
                var isIndex0 = body.Method.Name.IsAnyEqualsIgnoreCase("First", "FirstOrDefault");
                if (isIndex0)
                {
                    MemberExpression left = (MemberExpression)body.Arguments[0];
                    return new ResultSelector("{0}[0]".F(left.Member.Name));
                }

                var isByIndex = body.Method.Name.IsAnyEqualsIgnoreCase("ElementAt", "ElementAtOrDefault");
                if (isByIndex)
                {
                    MemberExpression left = (MemberExpression)body.Arguments[0];
                    ConstantExpression right = (ConstantExpression)body.Arguments[1];
                    return new ResultSelector("{0}[{1}]".F(left.Member.Name, right.Value));
                }          
            }
            if (property.Body.NodeType == ExpressionType.Convert)
            {
                var body = (MethodCallExpression)((UnaryExpression)property.Body).Operand;                
                var isAny = body.Method.Name.IsAnyEqualsIgnoreCase("Any");
                if (isAny)
                {
                    MemberExpression left = (MemberExpression)body.Arguments[0];
                    var right = new ConditionalIs(((LambdaExpression)body.Arguments[1]).Body, false,true);
                    dynamic data = right.GetData();
                    return new ResultSelector(string.Format("{0}.Any({1} {2} {3} {4})",left.Member.Name,data.left,data.method,data.right,data.inverse));
                }
            }

            return new ResultSelector(property.GetMemberName());
        }

        public ResultSelector For(string property)
        {
            return new ResultSelector(property);
        }

        #endregion
    }
}