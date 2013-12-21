namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    #endregion

    public class IncRadioButtonControl<TModel, TProperty> : IncControlBase
    {
        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        readonly IncLabelControl label;

        #endregion

        #region Constructors

        public IncRadioButtonControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
            this.label = new IncLabelControl(htmlHelper, property);
            this.label.AddClass("btn btn-default");
        }

        #endregion

        #region Properties

        public object Value { get; set; }

        public IncLabelControl Label { get { return this.label; } }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            Guard.NotNull("Value", Value);
            string value = Value.ToString();
            string displayName = this.label.Name ?? value;
            this.label.Name = this.htmlHelper.RadioButtonFor(this.property, value, GetAttributes()).ToHtmlString() + displayName;
            return this.label.ToHtmlString();
        }
    }
}