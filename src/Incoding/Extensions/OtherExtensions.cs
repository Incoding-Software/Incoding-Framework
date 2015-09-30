namespace Incoding.Extensions
{
    #region << Using >>

    using System;
    using System.Drawing;
    using System.Linq.Expressions;
    using System.Reflection;

    #endregion

    public static class OtherExtensions
    {

        public static bool IsEmpty(this Guid? value)
        {
            return value.GetValueOrDefault(Guid.Empty) == Guid.Empty;
        }

        public static string ToHex(this Color color)
        {
            return "#{0:X2}{1:X2}{2:X2}".F(color.R, color.G, color.B);
        }

        internal static Expression<Func<TInput, object>> ToBox<TInput, TOutput>(this Expression<Func<TInput, TOutput>> expression)
        {
            // Add the boxing operation, but get a weakly typed expression
            Expression converted = Expression.Convert(expression.Body, typeof(object));

            // Use Expression.Lambda to get back to strong typing
            return Expression.Lambda<Func<TInput, object>>
                    (converted, expression.Parameters);
        }
    }
}