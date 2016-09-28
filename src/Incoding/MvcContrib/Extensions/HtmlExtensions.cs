namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MvcContrib.MVD;
    using JetBrains.Annotations;

    #endregion

    public static class HtmlExtensions
    {
        [ThreadStatic]
        internal static HtmlHelper HtmlHelper;

        [ThreadStatic]
        internal static IUrlDispatcher UrlDispatcher;

        #region Factory constructors

        public static IDispatcher Dispatcher(this HtmlHelper htmlHelper)
        {
            HtmlHelper = htmlHelper;
            return IoCFactory.Instance.TryResolve<IDispatcher>();
        }

        // ReSharper disable once UnusedParameter.Global
        public static MvcHtmlString AsView<TData>(this IDispatcher dispatcher, TData data, [PathReference] string view, object model = null) where TData : class
        {
            Guard.NotNull("HtmlHelper", HtmlHelper, "HtmlHelper != null");
            return MvcHtmlString.Create(IoCFactory.Instance.TryResolve<ITemplateFactory>().Render(HtmlHelper, view, data, model));
        }

        public static MvcHtmlString AsViewFromQuery<TResult>(this IDispatcher dispatcher, QueryBase<TResult> query, [PathReference] string view, object model = null) where TResult : class
        {
            return dispatcher.AsView(dispatcher.Query(query), view, model);
        }

        [Obsolete("Please use AsView or AsViewFromQuery with Html.Dispatcher().AsView(data) / Html.Dispatcher().AsViewFromQuery(query)", true), ExcludeFromCodeCoverage, UsedImplicitly]
        public static MvcHtmlString AsView<TData>(this TData data, [PathReference] string view, object model = null) where TData : class
        {
            Guard.NotNull("HtmlHelper", HtmlHelper, "HtmlHelper != null");
            return MvcHtmlString.Create(IoCFactory.Instance.TryResolve<ITemplateFactory>().Render(HtmlHelper, view, data, model));
        }

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
            HtmlHelper = htmlHelper;
            return new IncodingHtmlHelper(htmlHelper);
        }

        public static SelectorHelper<TModel> Selector<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            HtmlHelper = htmlHelper;
            return new SelectorHelper<TModel>();
        }

        public static IIncodingMetaLanguageBindingDsl When(this HtmlHelper htmlHelper, JqueryBind bind)
        {
            return htmlHelper.When(bind.ToJqueryString());
        }

        public static IIncodingMetaLanguageBindingDsl When(this HtmlHelper htmlHelper, string bind)
        {
            HtmlHelper = htmlHelper;
            UrlDispatcher = new UrlDispatcher(new UrlHelper(htmlHelper.ViewContext.RequestContext));
            return new IncodingMetaLanguageDsl(bind);
        }

        #endregion
    }
}