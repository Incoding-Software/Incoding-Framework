namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Incoding.Extensions;

    #endregion

    public class IncLabelControl : IncControlBase
    {
        #region Fields

        readonly HtmlHelper htmlHelper;

        readonly string property;

        #endregion

        #region Constructors

        public IncLabelControl(HtmlHelper htmlHelper, LambdaExpression property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property.GetMemberName().Split(".".ToCharArray()).LastOrDefault();
            AddClass("control-label");
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        #endregion

        public override MvcHtmlString Render()
        {
            var tagBuilder = new TagBuilder("label");
            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(this.htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(this.property)));

            var metadata = ModelMetadata.FromStringExpression(this.property, this.htmlHelper.ViewData);
            string innerText = Name ?? metadata.DisplayName ?? this.property;
            tagBuilder.SetInnerText(innerText);

            tagBuilder.MergeAttributes(this.attributes, true);
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}