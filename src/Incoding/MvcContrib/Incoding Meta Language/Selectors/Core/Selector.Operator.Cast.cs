namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Web.Mvc;
    using System.Web.WebPages;
    using Incoding.Extensions;
    using StructureMap.Construction;

    #endregion

    public partial class Selector
    {
        #region Factory constructors

        public static object FromObject(object value)
        {
            var selector = value.GetType().IsImplement<Selector>() ? value : Value(value);
            return string.IsNullOrWhiteSpace(selector.ToString()) ? "''" : selector.ToString();
        }

        #endregion

        public static implicit operator string(Selector s)
        {
            return s.ToString();
        }

        public static implicit operator Selector(string s)
        {
            return Value(s);
        }


        public static implicit operator Selector(MvcHtmlString s)
        {
            return s.ToHtmlString();
        }

        public static implicit operator Selector(int s)
        {
            return Value(s);
        }

        public static implicit operator Selector(long s)
        {
            return Value(s);
        }

        public static implicit operator Selector(decimal s)
        {
            return Value(s);
        }

        public static implicit operator Selector(float s)
        {
            return Value(s);
        }

        public static implicit operator Selector(bool s)
        {
            return Value(s);
        }
    }
}