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

        string For(Expression<Func<TModel, object>> field);

        string For(Expression<Func<TModel, bool>> field);

        string Inline(Expression<Func<TModel, object>> field, string isTrue, string isFalse);

        string IsInline(Expression<Func<TModel, object>> field, string content);

        string NotInline(Expression<Func<TModel, object>> field, string content);

        string IsInline(Expression<Func<TModel, object>> field, MvcHtmlString content);

        string NotInline(Expression<Func<TModel, object>> field, MvcHtmlString content);

        string IsInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content);

        string NotInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content);

        string ForRaw(Expression<Func<TModel, object>> field);

        ITemplateSyntax<TNewModel> ForEach<TNewModel>(Expression<Func<TModel, IEnumerable<TNewModel>>> field);

        ITemplateSyntax<TModel> Is(Expression<Func<TModel, object>> field);

        ITemplateSyntax<TModel> Not(Expression<Func<TModel, object>> field);
    }
}