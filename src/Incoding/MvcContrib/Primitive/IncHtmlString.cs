namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Web.Mvc;
    using System.Web.WebPages;

    #endregion

    public struct IncHtmlString
    {
        private readonly MvcHtmlString result;

        public IncHtmlString(MvcHtmlString result)
        {
            this.result = result;
        }

        public override string ToString()
        {
            return this.result.ToHtmlString();
        }

        public static implicit operator string(IncHtmlString value)
        {
            return value.ToString();
        }

        public static implicit operator MvcHtmlString(IncHtmlString value)
        {
            return value.result;
        }

        public static implicit operator IncHtmlString(string content)
        {
            return new IncHtmlString(new MvcHtmlString(content));
        }

        public static implicit operator IncHtmlString(MvcHtmlString content)
        {
            return new IncHtmlString(content);
        }

        public static implicit operator IncHtmlString(Func<object, HelperResult> content)
        {
            return new IncHtmlString(new MvcHtmlString(content.Invoke(null).ToHtmlString()));
        }
    }
}