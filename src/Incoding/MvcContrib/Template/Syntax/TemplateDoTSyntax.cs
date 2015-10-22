namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.WebPages;
    using Incoding.Extensions;

    #endregion

    public class TemplateDoTSyntax<TModel> : ITemplateSyntax<TModel>
    {
        #region Fields

        readonly HtmlHelper htmlHelper;

        string property;

        #endregion

        #region Constructors

        public TemplateDoTSyntax(HtmlHelper htmlHelper, string property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;

            htmlHelper.ViewContext.Writer.Write("{{~it:x}}");
        }

        #endregion

        #region ITemplateSyntax<TModel> Members

        public ITemplateSyntax<TModel> Up()
        {
            throw new NotImplementedException();
        }

        public string For(string field)
        {
            return "{{=x." + field + "}}";
        }

        public string For(Expression<Func<TModel, object>> field)
        {
            return For(field.GetMemberName());
        }

        public string For(Expression<Func<TModel, bool>> field)
        {
            return For(field.GetMemberName());
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, string isFalse)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, MvcHtmlString isTrue, MvcHtmlString isFalse)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, MvcHtmlString isFalse)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, MvcHtmlString isTrue, string isFalse)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, Func<object, HelperResult> isFalse)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, MvcHtmlString isFalse)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, MvcHtmlString isTrue, Func<object, HelperResult> isFalse)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, string isTrue, Func<object, HelperResult> isFalse)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString Inline(Expression<Func<TModel, object>> field, Func<object, HelperResult> isTrue, string isFalse)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString IsInline(Expression<Func<TModel, object>> field, string content)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString IsInline(Expression<Func<TModel, object>> field, MvcHtmlString content)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString IsInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString NotInline(Expression<Func<TModel, object>> field, string content)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString NotInline(Expression<Func<TModel, object>> field, MvcHtmlString content)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString NotInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString ForRaw(string field)
        {
            throw new NotImplementedException();
        }

        public MvcHtmlString ForRaw(Expression<Func<TModel, object>> field)
        {
            throw new NotImplementedException();
        }

        public ITemplateSyntax<TNewModel> ForEach<TNewModel>(Expression<Func<TModel, IEnumerable<TNewModel>>> field)
        {
            throw new NotImplementedException();
        }

        public IDisposable Is(Expression<Func<TModel, object>> field)
        {
            throw new NotImplementedException();
        }

        public IDisposable Not(Expression<Func<TModel, object>> field)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            htmlHelper.ViewContext.Writer.Write("{{~}}");
        }

        #endregion

        #region Enums

        public enum DoTType
        { }

        #endregion
    }
}