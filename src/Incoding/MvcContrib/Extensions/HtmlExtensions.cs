namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    #endregion

    public static class HtmlExtensions
    {
        #region Factory constructors

        public static IncodingHtmlHelperFor<TModel, TProperty> For<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            return new IncodingHtmlHelperFor<TModel, TProperty>(htmlHelper, property);
        }

        public static IncodingHtmlHelperForGroup<TModel, TProperty> ForGroup<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            return new IncodingHtmlHelperForGroup<TModel, TProperty>(htmlHelper, property);
        }

        public static IncodingHtmlHelper Incoding<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new IncodingHtmlHelper(htmlHelper);
        }

        public static SelectorHelper<TModel> Selector<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new SelectorHelper<TModel>();
        }

        public static IIncodingMetaLanguageBindingDsl When(this HtmlHelper htmlHelper, JqueryBind bind)
        {
            return new IncodingMetaLanguageDsl(bind);
        }

        public static IIncodingMetaLanguageBindingDsl When(this HtmlHelper htmlHelper, string bind)
        {
            return new IncodingMetaLanguageDsl(bind);
        }


        #endregion
    }
}