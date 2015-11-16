namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.WebPages;

    #endregion

    public interface ITemplateSyntax<TModel> : IDisposable
    {
        ITemplateSyntax<TModel> Up();

        string For(string field);

        string For(Expression<Func<TModel, object>> field);

        string For(Expression<Func<TModel, bool>> field);

        MvcHtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, string isFalse);

        MvcHtmlString Inline(Expression<Func<TModel, object>> field, MvcHtmlString isTrue, MvcHtmlString isFalse);

        MvcHtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, MvcHtmlString isFalse);

        MvcHtmlString Inline(Expression<Func<TModel, object>> field, MvcHtmlString isTrue, string isFalse);

        MvcHtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, Func<object, HelperResult> isFalse);

        MvcHtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, MvcHtmlString isFalse);

        MvcHtmlString Inline(Expression<Func<TModel, object>> field, MvcHtmlString isTrue, Func<object, HelperResult> isFalse);

        MvcHtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, Func<object, HelperResult> isFalse);

        MvcHtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, string isFalse);

        MvcHtmlString IsInline(Expression<Func<TModel, object>> field, string content);

        MvcHtmlString IsInline(Expression<Func<TModel, object>> field, MvcHtmlString content);

        MvcHtmlString IsInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content);

        MvcHtmlString NotInline(Expression<Func<TModel, object>> field, string content);

        MvcHtmlString NotInline(Expression<Func<TModel, object>> field, MvcHtmlString content);

        MvcHtmlString NotInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content);

        MvcHtmlString ForRaw(string field);

        MvcHtmlString ForRaw(Expression<Func<TModel, object>> field);

        ITemplateSyntax<TNewModel> ForEach<TNewModel>(Expression<Func<TModel, IEnumerable<TNewModel>>> field);

        IDisposable Is(Expression<Func<TModel, object>> field);

        IDisposable Not(Expression<Func<TModel, object>> field);
    }
}