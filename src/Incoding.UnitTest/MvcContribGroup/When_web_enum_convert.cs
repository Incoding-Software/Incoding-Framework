namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(HtmlType))]
    public class When_web_enum_convert
    {
        It should_be_verify_ToStringHtmlContent = () =>
                                                      {
                                                          HtmlType.TextCss.ToLocalization().ShouldEqual("text/css");
                                                          HtmlType.TextHtml.ToLocalization().ShouldEqual("text/html");
                                                          HtmlType.TextXml.ToLocalization().ShouldEqual("text/xml");
                                                          HtmlType.TextJavaScript.ToLocalization().ShouldEqual("text/javascript");
                                                          HtmlType.TextTemplate.ToLocalization().ShouldEqual("text/template");
                                                      };
    }
}