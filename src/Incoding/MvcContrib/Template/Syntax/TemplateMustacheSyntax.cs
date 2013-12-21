namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.WebPages;
    using Incoding.Extensions;

    #endregion

    public class TemplateMustacheSyntax<TModel> : ITemplateSyntax<TModel>
    {
        #region Fields

        readonly HtmlHelper htmlHelper;

        readonly string property;

        #endregion

        #region Constructors

        public TemplateMustacheSyntax(HtmlHelper htmlHelper, string property, bool positiveConditional)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
            string typeConditional = positiveConditional ? "#" : "^";
            htmlHelper.ViewContext.Writer.Write("{{" + typeConditional + property + "}}");
        }

        #endregion

        ////ncrunch: no coverage start
        #region ITemplateSyntax<TModel> Members

        public ITemplateSyntax<TModel> Up()
        {
            throw new NotImplementedException();
        }
        ////ncrunch: no coverage end
        
        public string For(Expression<Func<TModel, object>> field)
        {
            return "{{" + field.GetMemberName() + "}}";
        }

        public string For(Expression<Func<TModel, bool>> field)
        {
            return "{{" + field.GetMemberName() + "}}";
        }

        public string Inline(Expression<Func<TModel, object>> field, string isTrue, string isFalse)
        {
            return IsInline(field, isTrue) + NotInline(field, isFalse);
        }

        public string IsInline(Expression<Func<TModel, object>> field, MvcHtmlString content)
        {
            string memberName = field.GetMemberName();
            return "{{#" + memberName + "}}" + content.ToHtmlString() + "{{/" + memberName + "}}";
        }

        public string NotInline(Expression<Func<TModel, object>> field, MvcHtmlString content)
        {
            string memberName = field.GetMemberName();
            return "{{^" + memberName + "}}" + content.ToHtmlString() + "{{/" + memberName + "}}";
        }

        public string IsInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            return IsInline(field, new MvcHtmlString(content.Invoke(null).ToHtmlString()));
        }

        public string NotInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            return NotInline(field, new MvcHtmlString(content.Invoke(null).ToHtmlString()));
        }

        public string ForRaw(Expression<Func<TModel, object>> field)
        {
            return "{{{" + field.GetMemberName() + "}}}";
        }

        public ITemplateSyntax<TNewModel> ForEach<TNewModel>(Expression<Func<TModel, IEnumerable<TNewModel>>> field)
        {
            return new TemplateMustacheSyntax<TNewModel>(this.htmlHelper, field.GetMemberName(), true);
        }

        public ITemplateSyntax<TModel> Is(Expression<Func<TModel, object>> field)
        {
            return new TemplateMustacheSyntax<TModel>(this.htmlHelper, field.GetMemberName(), true);
        }

        public ITemplateSyntax<TModel> Not(Expression<Func<TModel, object>> field)
        {
            return new TemplateMustacheSyntax<TModel>(this.htmlHelper, field.GetMemberName(), false);
        }

        public string IsInline(Expression<Func<TModel, object>> field, string content)
        {
            return IsInline(field, MvcHtmlString.Create(content));
        }

        public string NotInline(Expression<Func<TModel, object>> field, string content)
        {
            return NotInline(field, MvcHtmlString.Create(content));
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            this.htmlHelper.ViewContext.Writer.Write("{{/" + this.property + "}}");
        }

        #endregion
    }
}