namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.WebPages;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public static class RouteValueDictionaryExtensions
    {
        
        
        ////ncrunch: no coverage start
        public static BeginTag ToBeginForm(this RouteValueDictionary htmlAttributes, HtmlHelper htmlHelper, string url)
        {
            htmlAttributes.Set("method", HttpVerbs.Post.ToStringLower());
            htmlAttributes.Set("action", url);
            return new BeginTag(htmlHelper, HtmlTag.Form, htmlAttributes);
        }

        public static BeginTag ToBeginTag(this RouteValueDictionary htmlAttributes, HtmlHelper htmlHelper, HtmlTag tag)
        {
            return new BeginTag(htmlHelper, tag, htmlAttributes);
        }
        ////ncrunch: no coverage end
        
        public static MvcHtmlString ToButton(this RouteValueDictionary htmlAttributes)
        {
            return htmlAttributes.ToButton(string.Empty);
        }

        public static MvcHtmlString ToButton(this RouteValueDictionary htmlAttributes, string value)
        {
            return htmlAttributes.ToButton(new MvcHtmlString(value));
        }

        public static MvcHtmlString ToButton(this RouteValueDictionary htmlAttributes, Func<object, HelperResult> text)
        {
            return htmlAttributes.ToButton(Selector.FromHelperResult(text).ToString());
        }

        public static MvcHtmlString ToButton(this RouteValueDictionary htmlAttributes, MvcHtmlString value)
        {
            var tagBuilder = new TagBuilder(HtmlTag.Button.ToStringLower());
            tagBuilder.MergeAttributes(htmlAttributes, true);
            tagBuilder.InnerHtml = value.ToHtmlString();
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
            return htmlAttributes.ToI(string.Empty);
        }

        public static MvcHtmlString ToI(this RouteValueDictionary htmlAttributes, MvcHtmlString content)
        {
            return htmlAttributes.ToTag(HtmlTag.I, content);
        }

        public static MvcHtmlString ToI(this RouteValueDictionary htmlAttributes, string content)
        {
            return htmlAttributes.ToI(new MvcHtmlString(content));
        }

        public static MvcHtmlString ToI(this RouteValueDictionary htmlAttributes, Func<object, HelperResult> text)
        {
            return htmlAttributes.ToI(Selector.FromHelperResult(text).ToString());
        }

        public static MvcHtmlString ToImg(this RouteValueDictionary htmlAttributes)
        {
            return htmlAttributes.ToImg(string.Empty);
        }

        public static MvcHtmlString ToImg(this RouteValueDictionary htmlAttributes, string content)
        {
            return htmlAttributes.ToImg(new MvcHtmlString(content));
        }

        public static MvcHtmlString ToImg(this RouteValueDictionary htmlAttributes, MvcHtmlString content)
        {
            return htmlAttributes.ToTag(HtmlTag.Img, content);
        }

        public static MvcHtmlString ToImg(this RouteValueDictionary htmlAttributes, Func<object, HelperResult> text)
        {
            return htmlAttributes.ToImg(Selector.FromHelperResult(text).ToString());
        }

        public static MvcHtmlString ToInput(this RouteValueDictionary htmlAttributes, HtmlInputType inputType, string value)
        {
            var input = new TagBuilder(HtmlTag.Input.ToStringLower());
            input.MergeAttribute(HtmlAttribute.Type.ToLocalization().ToLower(), inputType.ToStringLower());
            if (!string.IsNullOrWhiteSpace(value))
                input.MergeAttribute(HtmlAttribute.Value.ToStringLower(), value);

            input.MergeAttributes(htmlAttributes, true);
            return new MvcHtmlString(input.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString ToLabel(this RouteValueDictionary htmlAttributes)
        {
            return htmlAttributes.ToLabel(string.Empty);
        }

        public static MvcHtmlString ToLabel(this RouteValueDictionary htmlAttributes, string content)
        {
            return htmlAttributes.ToLabel(new MvcHtmlString(content));
        }

        public static MvcHtmlString ToLabel(this RouteValueDictionary htmlAttributes, Func<object, HelperResult> text)
        {
            return htmlAttributes.ToLabel(Selector.FromHelperResult(text).ToString());
        }

        public static MvcHtmlString ToLabel(this RouteValueDictionary htmlAttributes, MvcHtmlString content)
        {
            return htmlAttributes.ToTag(HtmlTag.Label, content);
        }

        public static MvcHtmlString ToLink(this RouteValueDictionary htmlAttributes)
        {
            return htmlAttributes.ToLink(string.Empty);
        }

        public static MvcHtmlString ToLink(this RouteValueDictionary htmlAttributes, string content)
        {
            return htmlAttributes.ToLink(new MvcHtmlString(content));
        }

        public static MvcHtmlString ToLink(this RouteValueDictionary htmlAttributes, Func<object, HelperResult> text)
        {
            return htmlAttributes.ToLink(Selector.FromHelperResult(text).ToString());
        }

        public static MvcHtmlString ToLink(this RouteValueDictionary htmlAttributes, MvcHtmlString content)
        {
            var tagBuilder = new TagBuilder(HtmlTag.A.ToStringLower());
            tagBuilder.InnerHtml = content.ToHtmlString();
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
            return htmlAttributes.ToTag(tag, string.Empty);
        }

        public static MvcHtmlString ToTag(this RouteValueDictionary htmlAttributes, HtmlTag tag, string content)
        {
            return htmlAttributes.ToTag(tag, new MvcHtmlString(content));
        }

        public static MvcHtmlString ToTag(this RouteValueDictionary htmlAttributes, HtmlTag tag, Func<object, HelperResult> text)
        {
            return htmlAttributes.ToTag(tag, Selector.FromHelperResult(text).ToString());
        }

        public static MvcHtmlString ToTag(this RouteValueDictionary htmlAttributes, HtmlTag tag, MvcHtmlString content)
        {
            bool isContent = !string.IsNullOrWhiteSpace(content.With(r => r.ToHtmlString()));
            var tagBuilder = new TagBuilder(tag.ToStringLower());
            tagBuilder.MergeAttributes(htmlAttributes, true);
            if (isContent)
                tagBuilder.InnerHtml = content.ToHtmlString();

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
        
   
    }
}