namespace Incoding.Extensions
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Reflection;

    #endregion

    public static class ExpressionEqualExtensions
    {
        #region Factory constructors

        public static FieldInfo GetFieldFromMember(MemberExpression memberExpression)
        {
            Guard.NotNull("memberExpression", memberExpression);
            Guard.NotNull("memberExpression.Member", memberExpression.Member);

            return memberExpression.Member.DeclaringType.GetField(memberExpression.Member.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "Positive false")]
        public static bool IsExpressionEqual(this Expression left, Expression right)
        {
            if (!left.IsReferenceEquals(right))
                return false;

            if (left.NodeType == ExpressionType.MemberAccess && right.NodeType == ExpressionType.Constant)
                return IsMemberVsConstant((MemberExpression)left, (ConstantExpression)right);

            if (left.NodeType == ExpressionType.Constant && right.NodeType == ExpressionType.MemberAccess)
                return IsMemberVsConstant((MemberExpression)right, (ConstantExpression)left);

            if (left.NodeType != right.NodeType)
                return false;

            if (left.NodeType == ExpressionType.Parameter)
                return IsParameter((ParameterExpression)left, (ParameterExpression)right);
            if (left.NodeType == ExpressionType.Invoke)
                return IsInvoke((InvocationExpression)left, (InvocationExpression)right);
            if (left.NodeType == ExpressionType.Convert)
                return IsConvert((UnaryExpression)left, (UnaryExpression)right);
            if (left.NodeType == ExpressionType.Constant)
                return IsConstant((ConstantExpression)left, (ConstantExpression)right);
            if (left.NodeType == ExpressionType.Call)
                return IsCall((MethodCallExpression)left, (MethodCallExpression)right);
            if (left.NodeType == ExpressionType.Lambda)
                return IsLambda((LambdaExpression)left, (LambdaExpression)right);
            if (left.NodeType.IsAnyEquals(ExpressionType.Equal, ExpressionType.NotEqual, ExpressionType.LessThan, ExpressionType.LessThanOrEqual, ExpressionType.GreaterThan, ExpressionType.GreaterThanOrEqual, ExpressionType.AndAlso, ExpressionType.OrElse))
                return IsEqual((BinaryExpression)left, (BinaryExpression)right);
            if (left.NodeType == ExpressionType.MemberAccess)
            {
                var leftTempMember = (MemberExpression)left;
                var rightTempMember = (MemberExpression)right;
                bool leftIsConstant = leftTempMember.Expression != null && leftTempMember.Expression.NodeType == ExpressionType.Constant;
                bool rigthIsConstant = rightTempMember.Expression != null && rightTempMember.Expression.NodeType == ExpressionType.Constant;

                if (leftIsConstant && rigthIsConstant)
                {
                    var leftTuple = new Tuple<string, ConstantExpression>(leftTempMember.Member.Name, (ConstantExpression)leftTempMember.Expression);
                    var rightTuple = new Tuple<string, ConstantExpression>(rightTempMember.Member.Name, (ConstantExpression)rightTempMember.Expression);
                    return IsConstantAsMember(leftTuple, rightTuple);
                }

                return IsMemberAccess(leftTempMember, rightTempMember);
            }

            return left.NodeType.ToString().Equals(right.NodeType.ToString());
        }

        #endregion

        static bool IsConstantAsMember(Tuple<string, ConstantExpression> leftTuple, Tuple<string, ConstantExpression> rightTuple)
        {
            var leftValue = leftTuple.Item2.Value.TryGetValue(leftTuple.Item1);
            var rightValue = rightTuple.Item2.Value.TryGetValue(rightTuple.Item1);
            return leftValue.Equals(rightValue);
        }

        static bool IsMemberVsConstant(MemberExpression left, ConstantExpression right)
        {
            if (left.Expression == null)
            {
                var fieldInfo = GetFieldFromMember(left);
                var test = fieldInfo.GetValue(null);
                return test.Equals(right.Value);
            }

            object value;
            if (left.Expression.NodeType == ExpressionType.MemberAccess)
            {
                var deepLeft = left.Expression as MemberExpression;

                if (deepLeft.Expression == null)
                {
                    var field = GetFieldFromMember(deepLeft);
                    value = field.GetValue(deepLeft);
                }
                else
                    value = deepLeft.Expression.TryGetValue("Value").TryGetValue(deepLeft.Member.Name);
            }
            else
                value = left.Expression.TryGetValue("Value");

            return value.TryGetValue(left.Member.Name).Equals(right.Value);
        }

        static bool IsParameter(ParameterExpression left, ParameterExpression right)
        {
            return left.Type == right.Type;
        }

        static bool IsInvoke(InvocationExpression left, InvocationExpression right)
        {
            if (left.Arguments.Count != right.Arguments.Count)
                return false;

            for (int i = 0; i < left.Arguments.Count; i++)
            {
                var leftArgument = left.Arguments[i];
                var rightArgument = right.Arguments[i];
                if (!leftArgument.IsExpressionEqual(rightArgument))
                    return false;
            }

            return true;
        }

        static bool IsConvert(UnaryExpression left, UnaryExpression right)
        {
            if (left.Method != null && right.Method != null)
            {
                if (!left.Method.Name.Equals(right.Method.Name))
                    return false;
            }

            return left.Operand.IsExpressionEqual(right.Operand);
        }

        static bool IsConstant(ConstantExpression left, ConstantExpression right)
        {
            return left.Value.Equals(right.Value);
        }

        static bool IsCall(MethodCallExpression left, MethodCallExpression right)
        {
            if (left.Object != null || right.Object != null)
            {
                if (!left.Object.IsExpressionEqual(right.Object))
                    return false;
            }

            return left.Method.Name.Equals(right.Method.Name);
        }

        static bool IsLambda(LambdaExpression left, LambdaExpression right)
        {
            return IsExpressionEqual(left.Body, right.Body);
        }

        static bool IsEqual(BinaryExpression left, BinaryExpression right)
        {
            if (!left.Method.IsReferenceEquals(right.Method))
                return false;

            if (left.Method != null && right.Method != null)
            {
                if (!left.Method.Name.Equals(right.Method.Name))
                    return false;
            }

            return left.Left.IsExpressionEqual(right.Left) && left.Right.IsExpressionEqual(right.Right);
        }

        static bool IsMemberAccess(MemberExpression left, MemberExpression right)
        {
            if (left.Expression != null && right.Expression != null)
            {
                if (!left.Expression.IsExpressionEqual(right.Expression))
                    return false;
            }

            return left.Member.Equals(right.Member);
        }
    }
}