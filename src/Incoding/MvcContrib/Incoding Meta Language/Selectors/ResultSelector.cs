namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.Extensions;

    #endregion

    public class ResultSelector : Selector
    {
        public static bool operator !=(ResultSelector left, JquerySelectorExtend right)
        {
            return true;
        }

        public static bool operator ==(ResultSelector left, JquerySelectorExtend right)
        {
            return !(left != right);
        }

        public static bool operator <=(ResultSelector left, JquerySelectorExtend right)
        {
            return true;
        }

        public static bool operator >=(ResultSelector left, JquerySelectorExtend right)
        {
            return true;
        }



        internal const string TypeOfResult = "result";

        internal const string TypeOfEvent = "resultOfevent";

        private readonly string currentType;

        #region Constructors

        internal ResultSelector(string type, string prop)
                : base("||{0}*{1}||".F(type, prop))
        {
            this.currentType = type;
        }

        #endregion

        #region Api Methods

        public ResultSelector For<T>(Expression<Func<T, object>> property)
        {
            bool isMethod = property.Body is MethodCallExpression;
            if (isMethod)
            {
                var body = (MethodCallExpression)property.Body;
                bool isSelect = body.Method.Name.IsAnyEqualsIgnoreCase("Select");
                if (isSelect)
                {
                    var left = (MemberExpression)body.Arguments[0];
                    var right = (LambdaExpression)body.Arguments[1];
                    return new ResultSelector(this.currentType, "{0}.{1}({2})".F(left.Member.Name, body.Method.Name, ((MemberExpression)right.Body).Member.Name));
                }
                bool isIndex0 = body.Method.Name.IsAnyEqualsIgnoreCase("First", "FirstOrDefault");
                if (isIndex0)
                {
                    var left = (MemberExpression)body.Arguments[0];
                    return new ResultSelector(this.currentType, "{0}[0]".F(left.Member.Name));
                }

                bool isByIndex = body.Method.Name.IsAnyEqualsIgnoreCase("ElementAt", "ElementAtOrDefault");
                if (isByIndex)
                {
                    var left = (MemberExpression)body.Arguments[0];
                    var right = (ConstantExpression)body.Arguments[1];
                    return new ResultSelector(this.currentType, "{0}[{1}]".F(left.Member.Name, right.Value));
                }
            }
            if (property.Body.NodeType == ExpressionType.Convert)
            {
                var expression = (UnaryExpression)property.Body;
                if (expression.Operand.NodeType != ExpressionType.MemberAccess)
                {
                    var body = (MethodCallExpression)expression.Operand;
                    bool isAny = body.Method.Name.IsAnyEqualsIgnoreCase("Any");
                    if (isAny)
                    {
                        var left = (MemberExpression)body.Arguments[0];
                        var right = new ConditionalIs(((LambdaExpression)body.Arguments[1]).Body, false, true);
                        dynamic data = right.GetData();
                        return new ResultSelector(this.currentType, string.Format("{0}.Any({1} {2} {3} {4})", left.Member.Name, data.left, data.method, data.right, data.inverse));
                    }
                }
            }

            return new ResultSelector(this.currentType, property.GetMemberName());
        }

        public ResultSelector For(string property)
        {
            return new ResultSelector(this.currentType, property);
        }

        #endregion


    }
}