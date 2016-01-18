namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Web.Mvc;
    using System.Web.WebPages;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncSelectList))]
    public class When_inc_html_string_cast
    {
        It should_be_cast_inc_to_mvc_string = () =>
                                              {
                                                  MvcHtmlString htmlString = new IncHtmlString(new MvcHtmlString("Optional"));
                                                  htmlString.ToHtmlString().ShouldEqual("Optional");
                                              };

        It should_be_cast_inc_to_string = () =>
                                          {
                                              string htmlString = new IncHtmlString(new MvcHtmlString("Optional"));
                                              htmlString.ShouldEqual("Optional");
                                          };

        It should_be_cast_to_mvc_content_string = () =>
                                                  {
                                                      Func<object, HelperResult> converter = o => new HelperResult(writer => writer.WriteLine("<div>"));
                                                      IncHtmlString htmlString = converter;
                                                      htmlString.ToString().ShouldEqual(@"<div>
");
                                                  };

        It should_be_cast_to_mvc_html_string = () =>
                                               {
                                                   IncHtmlString htmlString = new MvcHtmlString("Optional");
                                                   htmlString.ToString().ShouldEqual("Optional");
                                               };

        It should_be_cast_to_string = () =>
                                      {
                                          IncHtmlString htmlString = "Optional";
                                          htmlString.ToString().ShouldEqual("Optional");
                                      };
    }
}