namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    #endregion

    public class IncCheckBoxControl<TModel, TProperty> : IncControlBase
    {
        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        readonly IncLabelControl label;

        #endregion

        #region Constructors

        public IncCheckBoxControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
            this.label = new IncLabelControl(htmlHelper, property);
            this.label.AddClass("checkbox");
        }

        #endregion

        #region Properties
        
        public IncLabelControl Label { get { return this.label; } }


        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            var metadata = ModelMetadata.FromLambdaExpression(this.property, this.htmlHelper.ViewData);
            bool? isChecked = null;
            bool result;
            if (metadata.Model != null && bool.TryParse(metadata.Model.ToString(), out result))
                isChecked = result;
            var checkBox = this.htmlHelper.CheckBox(ExpressionHelper.GetExpressionText(this.property), isChecked ?? false, GetAttributes());

            this.label.Name = checkBox.ToHtmlString() + this.label.Name;
            return this.label.ToHtmlString();
        }
    }
}