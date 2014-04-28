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

    public class TemplateHandlebarsSyntax<TModel> : ITemplateSyntax<TModel>
    {
        #region Fields

        readonly HtmlHelper htmlHelper;

        readonly string property;

        readonly HandlebarsType type;

        string level;

        #endregion

        #region Constructors

        public TemplateHandlebarsSyntax(HtmlHelper htmlHelper, string property, HandlebarsType type, string level)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
            this.type = type;
            this.level = level;

            htmlHelper.ViewContext.Writer.Write("{{#" + type.ToStringLower() + " " + this.level + this.property + "}}");
        }

        #endregion

        #region ITemplateSyntax<TModel> Members

        public ITemplateSyntax<TModel> Up()
        {
            this.level += "../";
            return this;
        }

        public string For(string field)
        {
            return Build("{{" + this.level + field + "}}");
        }

        public string For(Expression<Func<TModel, object>> field)
        {
            return For(field.GetMemberName());
        }

        public string For(Expression<Func<TModel, bool>> field)
        {
            return Build("{{#if " + this.level + field.GetMemberName() + "}}true{{else}}false{{/if}}");
        }

        public string Inline(Expression<Func<TModel, object>> field, string isTrue, string isFalse)
        {
            return Build("{{#if " + this.level + field.GetMemberName() + "}}" + isTrue + "{{else}}" + isFalse + "{{/if}}");
        }

        public string IsInline(Expression<Func<TModel, object>> field, MvcHtmlString content)
        {
            return IsInline(field, content.ToHtmlString());
        }

        public string NotInline(Expression<Func<TModel, object>> field, MvcHtmlString content)
        {
            return NotInline(field, content.ToHtmlString());
        }

        public string IsInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            return IsInline(field, content.Invoke(null).ToHtmlString());
        }

        public string NotInline(Expression<Func<TModel, object>> field, Func<object, HelperResult> content)
        {
            return NotInline(field, content.Invoke(null).ToHtmlString());
        }

        public string ForRaw(string field)
        {
            return Build("{{{" + this.level + field + "}}}");
        }

        public string ForRaw(Expression<Func<TModel, object>> field)
        {
            return ForRaw(field.GetMemberName());
        }

        public ITemplateSyntax<TNewModel> ForEach<TNewModel>(Expression<Func<TModel, IEnumerable<TNewModel>>> field)
        {
            return BuildNew<TNewModel>(field.GetMemberName(), HandlebarsType.Each);
        }

        public ITemplateSyntax<TModel> Is(Expression<Func<TModel, object>> field)
        {
            return BuildNew<TModel>(field.GetMemberName(), HandlebarsType.If);
        }

        public ITemplateSyntax<TModel> Not(Expression<Func<TModel, object>> field)
        {
            return BuildNew<TModel>(field.GetMemberName(), HandlebarsType.Unless);
        }

        public string IsInline(Expression<Func<TModel, object>> field, string content)
        {
            return Build("{{#if " + this.level + field.GetMemberName() + "}}" + content + "{{/if}}");
        }

        public string NotInline(Expression<Func<TModel, object>> field, string content)
        {
            return Build("{{#unless " + this.level + field.GetMemberName() + "}}" + content + "{{/unless}}");
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            this.htmlHelper.ViewContext.Writer.Write("{{/" + this.type.ToStringLower() + "}}");
        }

        #endregion

        ITemplateSyntax<T> BuildNew<T>(string newProperty, HandlebarsType newType)
        {
            var res = new TemplateHandlebarsSyntax<T>(this.htmlHelper, newProperty, newType, this.level);
            this.level = string.Empty;
            return res;
        }

        string Build(string res)
        {
            this.level = string.Empty;
            return res;
        }
    }

    #region Enums

    public enum HandlebarsType
    {
        If, 

        Unless, 

        Each, 
    }

    #endregion
}