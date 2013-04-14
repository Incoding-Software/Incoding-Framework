namespace Incoding.MvcContrib
{
    public static class WebEnumConvertToString
    {
        #region Factory constructors

        public static string ToHtmlType(HtmlType type)
        {
            switch (type)
            {
                case HtmlType.TextHtml:
                    return "text/html";
                case HtmlType.TextXml:
                    return "text/xml";
                case HtmlType.TextJavaScript:
                    return "text/javascript";
                case HtmlType.TextCss:
                    return "text/css";
                case HtmlType.TextJqueryTmpl:
                    return "text/x-jquery-tmpl";
                case HtmlType.TextMustacheTmpl:
                    return "text/x-mustache-tmpl";

                    ////ncrunch: no coverage start
                default:
                    return string.Empty;

                    ////ncrunch: no coverage end
            }
        }

        #endregion
    }
}