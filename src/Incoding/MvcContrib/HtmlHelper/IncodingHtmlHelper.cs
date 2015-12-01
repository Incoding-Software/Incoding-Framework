namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Block.IoC;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using JetBrains.Annotations;

    #endregion

    public class IncodingHtmlHelper
    {
        #region Fields

        readonly HtmlHelper htmlHelper;

        #endregion

        ////ncrunch: no coverage start

        #region Constructors

        public IncodingHtmlHelper(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        #endregion

        static TagBuilder CreateScript(string id, HtmlType type, string src, MvcHtmlString content)
        {
            var routeValueDictionary = new RouteValueDictionary(new { type = type.ToLocalization() });
            if (!string.IsNullOrWhiteSpace(src))
                routeValueDictionary.Merge(new { src });
            if (!string.IsNullOrWhiteSpace(id))
                routeValueDictionary.Merge(new { id });

            return CreateTag(HtmlTag.Script, content, routeValueDictionary);
        }

        static TagBuilder CreateInput(string value, string type, object attributes)
        {
            var routeValueDictionary = AnonymousHelper.ToDictionary(attributes);
            routeValueDictionary.Merge(new { value, type });
            var input = CreateTag(HtmlTag.Input, MvcHtmlString.Empty, routeValueDictionary);

            return input;
        }

        internal static TagBuilder CreateTag(HtmlTag tag, MvcHtmlString content, RouteValueDictionary attributes)
        {
            var tagBuilder = new TagBuilder(tag.ToStringLower());
            tagBuilder.InnerHtml = content.ReturnOrDefault(r => r.ToHtmlString(), string.Empty);
            tagBuilder.MergeAttributes(attributes, true);

            return tagBuilder;
        }

        // ReSharper disable ConvertToConstant.Global
        // ReSharper disable FieldCanBeMadeReadOnly.Global

        ////ncrunch: no coverage start

        #region Static Fields

        public static Selector DropDownTemplateId = "incodingDropDownTemplate".ToId();

        public static JqueryAjaxOptions DropDownOption = new JqueryAjaxOptions(JqueryAjaxOptions.Default);

        public static BootstrapOfVersion BootstrapVersion = BootstrapOfVersion.v2;

        public static B Def_Label_Class  = B.Col_xs_5;

        public static B Def_Control_Class = B.Col_xs_7;

        
        
        #endregion

        ////ncrunch: no coverage end

        // ReSharper restore FieldCanBeMadeReadOnly.Global
        // ReSharper restore ConvertToConstant.Global

        ////ncrunch: no coverage end

        #region Api Methods

        public MvcHtmlString Script([PathReference] string src)
        {
            var script = CreateScript(string.Empty, HtmlType.TextJavaScript, src, MvcHtmlString.Empty);
            return new MvcHtmlString(script.ToString());
        }

        [Obsolete("Please use Template")]
        public MvcScriptTemplate<TModel> ScriptTemplate<TModel>(string id)
        {
            return new MvcScriptTemplate<TModel>(this.htmlHelper, id);
        }

        public MvcTemplate<TModel> Template<TModel>()
        {
            return new MvcTemplate<TModel>(this.htmlHelper);
        }

        public MvcHtmlString Link([PathReference] string href)
        {
            var tagBuilder = CreateTag(HtmlTag.Link, MvcHtmlString.Empty, new RouteValueDictionary());
            tagBuilder.MergeAttribute(HtmlAttribute.Href.ToStringLower(), href);
            tagBuilder.MergeAttribute(HtmlAttribute.Rel.ToStringLower(), "stylesheet");
            tagBuilder.MergeAttribute(HtmlAttribute.Type.ToStringLower(), HtmlType.TextCss.ToLocalization());

            return new MvcHtmlString(tagBuilder.ToString());
        }

        public MvcHtmlString Button(string value, object attributes = null)
        {
            var button = CreateTag(HtmlTag.Button, new MvcHtmlString(value), AnonymousHelper.ToDictionary(attributes));
            return new MvcHtmlString(button.ToString());
        }

        public MvcHtmlString Submit(string value, object attributes = null)
        {
            var submit = CreateInput(value, HtmlInputType.Submit.ToStringLower(), attributes);
            return new MvcHtmlString(submit.ToString(TagRenderMode.SelfClosing));
        }

        public MvcHtmlString Img(string src, object attributes = null)
        {
            return Img(src, MvcHtmlString.Empty, attributes);
        }

        public MvcHtmlString Img(string src, MvcHtmlString content, object attributes = null)
        {
            var routeValueDictionary = AnonymousHelper.ToDictionary(attributes);
            routeValueDictionary.Merge(new { src });
            var img = CreateTag(HtmlTag.Img, content, routeValueDictionary);
            return new MvcHtmlString(img.ToString());
        }

        public MvcHtmlString Anchor(string href, string content, object attributes = null)
        {
            return Anchor(href, new MvcHtmlString(content), attributes);
        }

        public MvcHtmlString Anchor(string href, MvcHtmlString content, object attributes = null)
        {
            var routeValue = AnonymousHelper.ToDictionary(attributes);
            routeValue.Set("href", href);
            var a = CreateTag(HtmlTag.A, content, routeValue);
            return new MvcHtmlString(a.ToString());
        }

        public MvcHtmlString Div(MvcHtmlString content, object attributes = null)
        {
            var div = CreateTag(HtmlTag.Div, content, AnonymousHelper.ToDictionary(attributes));
            return new MvcHtmlString(div.ToString());
        }

        public MvcHtmlString Div(string content, object attributes = null)
        {
            return Div(new MvcHtmlString(content), attributes);
        }

        public MvcHtmlString Span(MvcHtmlString content, object attributes = null)
        {
            var tag = CreateTag(HtmlTag.Span, content, AnonymousHelper.ToDictionary(attributes));
            return new MvcHtmlString(tag.ToString());
        }

        public MvcHtmlString Span(string content, object attributes = null)
        {
            return Span(new MvcHtmlString(content), attributes);
        }

        public MvcHtmlString I(object attributes = null)
        {
            var tag = CreateTag(HtmlTag.I, MvcHtmlString.Empty, AnonymousHelper.ToDictionary(attributes));
            return new MvcHtmlString(tag.ToString());
        }

        public MvcHtmlString P(MvcHtmlString content, object attributes = null)
        {
            var tag = CreateTag(HtmlTag.P, content, AnonymousHelper.ToDictionary(attributes));
            return new MvcHtmlString(tag.ToString());
        }

        public MvcHtmlString P(string content, object attributes = null)
        {
            return P(new MvcHtmlString(content), attributes);
        }

        public MvcHtmlString Ul(MvcHtmlString content, object attributes = null)
        {
            var tag = CreateTag(HtmlTag.Ul, content, AnonymousHelper.ToDictionary(attributes));
            return new MvcHtmlString(tag.ToString());
        }

        public MvcHtmlString Ul(string content, object attributes = null)
        {
            return Ul(new MvcHtmlString(content), attributes);
        }

        public MvcHtmlString Li(MvcHtmlString content, object attributes = null)
        {
            var tag = CreateTag(HtmlTag.Li, content, AnonymousHelper.ToDictionary(attributes));
            return new MvcHtmlString(tag.ToString());
        }

        public MvcHtmlString Li(string content, object attributes = null)
        {
            return Li(new MvcHtmlString(content), attributes);
        }

        public MvcHtmlString Tag(HtmlTag tag, MvcHtmlString content, object attributes = null)
        {
            var res = CreateTag(tag, content, AnonymousHelper.ToDictionary(attributes));
            return new MvcHtmlString(res.ToString());
        }

        public BeginTag BeginTag(HtmlTag tag, object attributes = null)
        {
            return new BeginTag(htmlHelper, tag, AnonymousHelper.ToDictionary(attributes));
        }

        public MvcHtmlString Tag(HtmlTag tag, string content, object attributes = null)
        {
            return Tag(tag, new MvcHtmlString(content), attributes);
        }

        public MvcHtmlString File(string value, object attributes = null)
        {
            var file = CreateInput(value, HtmlInputType.File.ToStringLower(), attributes);
            return new MvcHtmlString(file.ToString(TagRenderMode.SelfClosing));
        }

        public MvcHtmlString RenderDropDownTemplate()
        {
            var templateFactory = IoCFactory.Instance.TryResolve<ITemplateFactory>() ?? new TemplateHandlebarsFactory();
            string template = templateFactory.GetDropDownTemplate();
            return new MvcHtmlString(CreateScript("incodingDropDownTemplate", HtmlType.TextTemplate, string.Empty, new MvcHtmlString(template)).ToString());
        }

        #endregion
    }

}