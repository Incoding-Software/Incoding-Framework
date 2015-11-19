namespace Incoding.MvcContrib
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Incoding.Maybe;

    public class IncStaticControl<TModel, TProperty> : IncControlBase
    {
        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncStaticControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
        }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            var tagBuilder = new TagBuilder("p");

            tagBuilder.InnerHtml = ModelMetadata
                    .FromLambdaExpression(property, htmlHelper.ViewData)
                    .Model.With(r => r.ToString());

            tagBuilder.MergeAttributes(attributes, true);
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}