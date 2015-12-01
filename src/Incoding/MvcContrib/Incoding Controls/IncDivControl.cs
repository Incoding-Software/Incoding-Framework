namespace Incoding.MvcContrib
{
    using System.Web.Mvc;
    using Incoding.Extensions;

    public class IncDivControl : IncControlBase
    {
        public MvcHtmlString Content { get; internal set; }


        public override MvcHtmlString ToHtmlString()
        {
            var tagBuilder = new TagBuilder(HtmlTag.Div.ToStringLower());

            tagBuilder.InnerHtml = Content.ToHtmlString();

            tagBuilder.MergeAttributes(this.attributes, true);
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}