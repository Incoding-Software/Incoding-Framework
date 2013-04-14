namespace Incoding.Extensions
{
    #region << Using >>

    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    #endregion

    public static class StringExtensions
    {
        #region Static Fields

        static readonly Regex regEmail = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

        static readonly Regex regGuid = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

        static readonly Regex regUrl = new Regex(@"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$");

        #endregion

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

        [DebuggerStepThrough]
        public static bool IsEmail(this string email)
        {
            return regEmail.IsMatch(email);
        }

        [DebuggerStepThrough]
        public static bool IsGuid(this string guid)
        {
            return regGuid.IsMatch(guid);
        }

        [DebuggerStepThrough]
        public static bool IsUrl(this string url)
        {
            return regUrl.IsMatch(url);
        }

        public static MvcHtmlString ToMvcHtmlString(this string value)
        {
            return new MvcHtmlString(value);
        }

        #endregion
    }
}