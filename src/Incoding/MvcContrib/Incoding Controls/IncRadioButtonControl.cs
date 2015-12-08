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
    public enum ModeOfRadio
    {
        Normal = 0,

        Inline = 1
    }


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
            string value = Value.With(r=>r.ToString());
            Guard.NotNullOrWhiteSpace("'value'", value);

            var div = new TagBuilder(HtmlTag.Div.ToStringLower());
            div.AddCssClass(Mode == ModeOfRadio.Normal ? B.Radio.AsClass() : B.Radio_inline.AsClass());
            div.AddCssClass(GetAttributes().GetOrDefault(HtmlAttribute.Class.ToStringLower(), string.Empty).ToString());
            var spanAsLabel = new TagBuilder(HtmlTag.Span.ToStringLower());
            spanAsLabel.InnerHtml = this.Label.Name ?? value;
            var label = new TagBuilder(HtmlTag.Label.ToStringLower());
            label.InnerHtml = this.htmlHelper.RadioButtonFor(this.property, value, GetAttributes()).ToHtmlString()
                              + new TagBuilder(HtmlTag.I.ToStringLower()).ToString(TagRenderMode.Normal)
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

        public ModeOfRadio Mode { get; set; }

        #endregion
    }
}