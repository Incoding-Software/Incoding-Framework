namespace Incoding.Extensions
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web.Mvc;

    #endregion

    public static class StringExtensions
    {
        #region Factory constructors

        [DebuggerStepThrough]
        public static bool EqualsWithInvariant(this string left, string right)
        {
            return left.Equals(right, StringComparison.InvariantCultureIgnoreCase);
        }

        [DebuggerStepThrough]
        public static string F(this string format, params object[] param)
        {
            return string.Format(format, param);
        }

        [DebuggerStepThrough]
        public static string AsString(this IEnumerable<object> values, string separate)
        {
            return string.Join(separate, values);
        }

        [DebuggerStepThrough]
        public static bool IsGuid(this string value)
        {
            Guid outGuid;
            return Guid.TryParse(value, out outGuid);
        }

        [DebuggerStepThrough]
        public static MvcHtmlString ToMvcHtmlString(this string value)
        {
            return new MvcHtmlString(value);
        }

        [DebuggerStepThrough]
        public static string ToSafeJSArgument(this string value)
        {
            return value.Replace("'", "\\'");
        }

        #endregion
    }
}