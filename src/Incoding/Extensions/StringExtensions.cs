namespace Incoding.Extensions
{
    #region << Using >>

    using System;
    using System.Web.Mvc;

    #endregion

    public static class StringExtensions
    {
        #region Factory constructors

        public static bool EqualsWithInvariant(this string left, string right)
        {
            return left.Equals(right, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string F(this string format, params object[] param)
        {
            return string.Format(format, param);
        }

        public static bool IsGuid(this string value)
        {
            Guid outGuid;
            return Guid.TryParse(value, out outGuid);
        }

        public static MvcHtmlString ToMvcHtmlString(this string value)
        {
            return new MvcHtmlString(value);
        }

        public static string ToSafeJSArgument(this string value)
        {
            return value.Replace("'", "\\'");
        }

        #endregion
    }
}