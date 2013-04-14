namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(WebEnumConvertToString))]
    public class When_web_enum_convert
    {
        It should_be_verify_ToStringHtmlContent = () =>
                                                      {
                                                          WebEnumConvertToString.ToHtmlType(HtmlType.TextCss).ShouldEqual("text/css");
                                                          WebEnumConvertToString.ToHtmlType(HtmlType.TextHtml).ShouldEqual("text/html");
                                                          WebEnumConvertToString.ToHtmlType(HtmlType.TextXml).ShouldEqual("text/xml");
                                                          WebEnumConvertToString.ToHtmlType(HtmlType.TextJavaScript).ShouldEqual("text/javascript");
                                                          WebEnumConvertToString.ToHtmlType(HtmlType.TextJqueryTmpl).ShouldEqual("text/x-jquery-tmpl");
                                                          WebEnumConvertToString.ToHtmlType(HtmlType.TextMustacheTmpl).ShouldEqual("text/x-mustache-tmpl");
                                                      };
    }
}