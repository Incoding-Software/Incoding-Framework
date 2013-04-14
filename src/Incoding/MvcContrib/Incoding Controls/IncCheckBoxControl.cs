namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Incoding.Extensions;

    #endregion

    public class IncCheckBoxControl<TModel, TProperty> : IncControlBase
    {
        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncCheckBoxControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        #endregion

        public override MvcHtmlString Render()
        {
            var metadata = ModelMetadata.FromLambdaExpression(this.property, this.htmlHelper.ViewData);
            bool? isChecked = null;
            bool result;
            if (metadata.Model != null && bool.TryParse(metadata.Model.ToString(), out result))
                isChecked = result;
            var checkBox = this.htmlHelper.CheckBox(ExpressionHelper.GetExpressionText(this.property), isChecked ?? false, this.attributes);

            var tagLabel = new TagBuilder(HtmlTag.Label.ToStringLower());
            tagLabel.AddCssClass("checkbox");
            tagLabel.InnerHtml = checkBox.ToHtmlString() + " " + Name;

            return new MvcHtmlString(tagLabel.ToString());
        }
    }
}