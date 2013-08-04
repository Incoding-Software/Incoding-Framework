namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Incoding.Extensions;

    #endregion

    public class MvcTemplate<TModel> : IDisposable
    {
        #region Fields

        readonly HtmlHelper htmlHelper;

        #endregion

        #region Constructors

        public MvcTemplate(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        #endregion

        #region Api Methods

        public MvcHtmlString Sum(Expression<Func<TModel, object>> property)
        {
            return new MvcHtmlString("{{#IncTemplateSum}}" + property.GetMemberName() + "{{/IncTemplateSum}}");
        }

        public MvcHtmlString Max(Expression<Func<TModel, object>> property)
        {
            return new MvcHtmlString("{{#IncTemplateMax}}" + property.GetMemberName() + "{{/IncTemplateMax}}");
        }

        public MvcHtmlString Min(Expression<Func<TModel, object>> property)
        {
            return new MvcHtmlString("{{#IncTemplateMin}}" + property.GetMemberName() + "{{/IncTemplateMin}}");
        }

        public MvcHtmlString First(Expression<Func<TModel, object>> property)
        {
            return new MvcHtmlString("{{#IncTemplateFirst}}" + property.GetMemberName() + "{{/IncTemplateFirst}}");
        }

        public MvcHtmlString Last(Expression<Func<TModel, object>> property)
        {
            return new MvcHtmlString("{{#IncTemplateLast}}" + property.GetMemberName() + "{{/IncTemplateLast}}");
        }

        public MvcHtmlString Index()
        {
            return new MvcHtmlString("{{IncTemplateIndex}}");
        }

        public MvcHtmlString Average(Expression<Func<TModel, object>> property)
        {
            return new MvcHtmlString("{{#IncTemplateAverage}}" + property.GetMemberName() + "{{/IncTemplateAverage}}");
        }

        public ITemplateSyntax<TModel> ForEach()
        {
            return new TemplateMustacheSyntax<TModel>(this.htmlHelper, "data", true);
        }

        public ITemplateSyntax<TModel> NotEach()
        {
            return new TemplateMustacheSyntax<TModel>(this.htmlHelper, "data", false);
        }

        public MvcHtmlString Count()
        {
            return new MvcHtmlString("{{#IncTemplateCount}}Count{{/IncTemplateCount}}");
        }

        #endregion

        #region Disposable

        public virtual void Dispose() { }

        #endregion
    }
}