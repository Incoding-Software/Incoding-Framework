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
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            var tagBuilder = new TagBuilder("label");
            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(property)));

            var metadata = ModelMetadata.FromStringExpression(property, htmlHelper.ViewData);
            string innerText = Name ?? metadata.DisplayName ?? property;
            tagBuilder.InnerHtml = innerText;

            tagBuilder.MergeAttributes(attributes, true);
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}