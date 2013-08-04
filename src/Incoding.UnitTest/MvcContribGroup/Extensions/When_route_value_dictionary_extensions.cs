namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Web.Routing;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(RouteValueDictionaryExtensions))]
    public class When_route_value_dictionary_extensions
    {
        It should_be_to_tag = () => new RouteValueDictionary(new { @class = "class" })
                                            .ToTag(HtmlTag.Label)
                                            .ToHtmlString()
                                            .ShouldEqual("<label class=\"class\"></label>");

        It should_be_to_div = () => new RouteValueDictionary(new { @class = "class" })
                                            .ToDiv()
                                            .ToHtmlString()
                                            .ShouldEqual("<div class=\"class\"></div>");

        It should_be_to_text_area = () => new RouteValueDictionary(new { @class = "class" })
                                                  .ToTextArea()
                                                  .ToHtmlString()
                                                  .ShouldEqual("<textarea class=\"class\"></textarea>");

        It should_be_to_i = () => new RouteValueDictionary(new { @class = "class" })
                                          .ToI()
                                          .ToHtmlString()
                                          .ShouldEqual("<i class=\"class\"></i>");

        It should_be_to_button = () => new RouteValueDictionary(new { @class = "class" })
                                               .ToButton("Value")
                                               .ToHtmlString()
                                               .ShouldEqual("<button class=\"class\">Value</button>");

        It should_be_to_submit = () => new RouteValueDictionary(new { @class = "class" })
                                               .ToSubmit("Value")
                                               .ToHtmlString()
                                               .ShouldEqual("<input class=\"class\" type=\"submit\" value=\"Value\" />");

        It should_be_to_text_box = () => new RouteValueDictionary(new { @class = "class" })
                                                 .ToTextBox("Value")
                                                 .ToHtmlString()
                                                 .ShouldEqual("<input class=\"class\" type=\"text\" value=\"Value\" />");

        It should_be_to_checkbox_checked = () => new RouteValueDictionary(new { @class = "class" })
                                                         .ToCheckBox(true)
                                                         .ToHtmlString()
                                                         .ShouldEqual("<input checked=\"checked\" class=\"class\" type=\"checkbox\" />");

        It should_be_to_checkbox_un_checked = () => new RouteValueDictionary(new { @class = "class" })
                                                            .ToCheckBox(false)
                                                            .ToHtmlString()
                                                            .ShouldEqual("<input class=\"class\" type=\"checkbox\" />");

        It should_be_to_select = () => new RouteValueDictionary(new { @class = "class" })
                                               .ToSelect()
                                               .ToHtmlString()
                                               .ShouldEqual("<select class=\"class\"></select>");

        It should_be_to_link = () => new RouteValueDictionary(new { @class = "class" })
                                             .ToLink("Value")
                                             .ToHtmlString()
                                             .ShouldEqual("<a class=\"class\" href=\"javascript:void(0);\">Value</a>");

        It should_be_to_link_no_replace_href = () => new RouteValueDictionary(new { @href = "http://asdsd" })
                                                             .ToLink("Value")
                                                             .ToHtmlString()
                                                             .ShouldEqual("<a href=\"http://asdsd\">Value</a>");
    }
}