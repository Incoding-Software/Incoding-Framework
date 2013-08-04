namespace Incoding.Extensions
{
    #region << Using >>

    using System;
    using System.Diagnostics;
    using System.Web.Mvc;

    #endregion

    public static class StringExtensions
    {
        #region Factory constructors

        [DebuggerStepThrough]
        public static string ApplyFormat(this string format, params object[] param)
        {
            return string.Format(format, param);
        }

        [DebuggerStepThrough]
        public static bool EqualsWithInvariant(this string left, string right)
        {
            return left.Equals(right, StringComparison.InvariantCultureIgnoreCase);
        }

        [DebuggerStepThrough]
        public static string F(this string format, params object[] param)
        {
            return format.ApplyFormat(param);
        }

        public static MvcHtmlString ToMvcHtmlString(this string value)
        {
            return new MvcHtmlString(value);
        }

        #endregion
    }
}