namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Extensions;

    #endregion

    public static class RouteValueDictionaryExtensions
    {
        ////ncrunch: no coverage start
        #region Factory constructors

        public static BeginTag ToBeginTag(this RouteValueDictionary htmlAttributes, HtmlHelper htmlHelper, HtmlTag tag)
        {
            return new BeginTag(htmlHelper, tag, htmlAttributes);
        }

        ////ncrunch: no coverage end
        public static MvcHtmlString ToButton(this RouteValueDictionary htmlAttributes, string value)
        {
            var tagBuilder = new TagBuilder(HtmlTag.Button.ToStringLower());
            tagBuilder.MergeAttributes(htmlAttributes, true);
            tagBuilder.SetInnerText(value);
            return new MvcHtmlString(tagBuilder.ToString());
        }

        public static MvcHtmlString ToCheckBox(this RouteValueDictionary htmlAttributes, bool value)
        {
            if (value)
                htmlAttributes.Set(HtmlAttribute.Checked.ToStringLower(), "checked");

            return htmlAttributes.ToInput(HtmlInputType.CheckBox, string.Empty);
        }

        public static MvcHtmlString ToDiv(this RouteValueDictionary htmlAttributes)
        {
            return htmlAttributes.ToTag(HtmlTag.Div);
        }

        public static MvcHtmlString ToI(this RouteValueDictionary htmlAttributes)
        {
            return htmlAttributes.ToTag(HtmlTag.I);
        }

        public static MvcHtmlString ToInput(this RouteValueDictionary htmlAttributes, HtmlInputType inputType, string value)
        {
            var input = new TagBuilder(HtmlTag.Input.ToStringLower());
            input.MergeAttribute(HtmlAttribute.Type.ToStringLower(), inputType.ToStringLower());
            if (!string.IsNullOrWhiteSpace(value))
                input.MergeAttribute(HtmlAttribute.Value.ToStringLower(), value);

            input.MergeAttributes(htmlAttributes, true);
            return new MvcHtmlString(input.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString ToLink(this RouteValueDictionary htmlAttributes, string content)
        {
            var tagBuilder = new TagBuilder(HtmlTag.A.ToStringLower());
            tagBuilder.SetInnerText(content);
            tagBuilder.MergeAttribute(HtmlAttribute.Href.ToStringLower(), "javascript:void(0);", false);

            tagBuilder.MergeAttributes(htmlAttributes, true);
            return new MvcHtmlString(tagBuilder.ToString());
        }

        public static MvcHtmlString ToSelect(this RouteValueDictionary htmlAttributes)
        {
            var select = new TagBuilder(HtmlTag.Select.ToStringLower());
            select.MergeAttributes(htmlAttributes, true);
            return new MvcHtmlString(select.ToString());
        }

        public static MvcHtmlString ToSubmit(this RouteValueDictionary htmlAttributes, string value)
        {
            return htmlAttributes.ToInput(HtmlInputType.Submit, value);
        }

        public static MvcHtmlString ToTag(this RouteValueDictionary htmlAttributes, HtmlTag tag)
        {
            var tagBuilder = new TagBuilder(tag.ToStringLower());
            tagBuilder.MergeAttributes(htmlAttributes, true);
            return new MvcHtmlString(tagBuilder.ToString());
        }

        public static MvcHtmlString ToTextArea(this RouteValueDictionary htmlAttributes)
        {
            return htmlAttributes.ToTag(HtmlTag.TextArea);
        }

        public static MvcHtmlString ToTextBox(this RouteValueDictionary htmlAttributes, string value = "")
        {
            return htmlAttributes.ToInput(HtmlInputType.Text, value);
        }

        #endregion

        ////ncrunch: no coverage start

        ////ncrunch: no coverage end        
    }
}