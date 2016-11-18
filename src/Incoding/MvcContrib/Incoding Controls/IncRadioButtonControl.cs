namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class IncRadioButtonControl<TModel, TProperty> : IncControlBase
    {
        #region Constructors

        public IncRadioButtonControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
            this.Label = new IncLabelControl(htmlHelper, property);
        }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            string value = Value.With(r => r.ToString());
            Guard.NotNullOrWhiteSpace("value", value, errorMessage: "Please set Value like are setting.Value = something");

            var div = new TagBuilder(HtmlTag.Div.ToStringLower());
            div.AddCssClass(Mode == ModeOfRadio.Normal ? B.Radio.ToLocalization() : B.Radio_inline.ToLocalization());
            var parentClass = GetAttributes().GetOrDefault(HtmlAttribute.Class.ToStringLower(), string.Empty).ToString();
            if (!string.IsNullOrEmpty(parentClass))
                div.AddCssClass(parentClass);
            var spanAsLabel = new TagBuilder(HtmlTag.Span.ToStringLower());
            spanAsLabel.InnerHtml = this.Label.Name ?? value;
            var label = new TagBuilder(HtmlTag.Label.ToStringLower());
            var icon = new TagBuilder(HtmlTag.I.ToStringLower());
            if (!string.IsNullOrWhiteSpace(IconClass))
                icon.AddCssClass(IconClass);

            label.InnerHtml = this.htmlHelper.RadioButtonFor(this.property, value, GetAttributes()).ToHtmlString()
                              + icon.ToString(TagRenderMode.Normal)
                              + spanAsLabel.ToString(TagRenderMode.Normal);
            div.InnerHtml = label.ToString(TagRenderMode.Normal);

            return new MvcHtmlString(div.ToString(TagRenderMode.Normal));
        }

        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Properties

        public object Value { get; set; }

        public IncLabelControl Label { get; protected set; }

        public string IconClass { get; set; }

        public ModeOfRadio Mode { get; set; }

        #endregion
    }
}