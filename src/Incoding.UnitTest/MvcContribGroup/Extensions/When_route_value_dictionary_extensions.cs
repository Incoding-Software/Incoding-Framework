namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.WebPages;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(RouteValueDictionaryExtensions))]
    public class When_route_value_dictionary_extensions
    {
        It should_be_to_begin_form = () =>
                                    {
                                        var htmlHelper = MockHtmlHelper<object>
                                                .When();
                                        typeof(HtmlExtensions).GetField("HtmlHelper", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, htmlHelper.OriginalNoGeneric);

                                        var form = new RouteValueDictionary(new { @class = "class" })
                                                .ToBeginForm(Pleasure.Generator.TheSameString());
                                        form.Dispose();

                                        htmlHelper.ShouldBeWriter("<form class=\"class\" method=\"post\" enctype=\"application/x-www-form-urlencoded\" action=\"TheSameString\" >");
                                        htmlHelper.ShouldBeWriter("</form>");
                                    };     
        

        It should_be_to_button = () => new RouteValueDictionary(new { @class = "class" })
                                               .ToButton()
                                               .ToHtmlString()
                                               .ShouldEqual("<button class=\"class\"></button>");

        It should_be_to_button_with_content = () => new RouteValueDictionary(new { @class = "class" })
                                                            .ToButton("Value")
                                                            .ToHtmlString()
                                                            .ShouldEqual("<button class=\"class\">Value</button>");

        It should_be_to_button_with_content_as_func = () => new RouteValueDictionary(new { @class = "class" })
                                                                    .ToButton(o => new HelperResult(writer => writer.WriteLine("<div>")))
                                                                    .ToHtmlString()
                                                                    .ShouldEqual("<button class=\"class\"><div></button>"); //-V3050

        It should_be_to_checkbox_checked = () => new RouteValueDictionary(new { @class = "class" })
                                                         .ToCheckBox(true)
                                                         .ToHtmlString()
                                                         .ShouldEqual("<input checked=\"checked\" class=\"class\" type=\"checkbox\" />");

        It should_be_to_checkbox_un_checked = () => new RouteValueDictionary(new { @class = "class" })
                                                            .ToCheckBox(false)
                                                            .ToHtmlString()
                                                            .ShouldEqual("<input class=\"class\" type=\"checkbox\" />");

        It should_be_to_div = () => new RouteValueDictionary(new { @class = "class" })
                                            .ToDiv()
                                            .ToHtmlString()
                                            .ShouldEqual("<div class=\"class\"></div>");

        It should_be_to_div_mvchtml_content = () => new RouteValueDictionary()
                                                            .ToDiv(new MvcHtmlString("<p>Test</p>"))
                                                            .ToHtmlString()
                                                            .ShouldEqual("<div><p>Test</p></div>");

        It should_be_to_div_result_content = () => new RouteValueDictionary()
                                                           .ToDiv(o => new HelperResult(writer => writer.WriteLine("Test")))
                                                           .ToHtmlString()
                                                           .ShouldEqual("<div>Test</div>");

        It should_be_to_div_string_content = () => new RouteValueDictionary()
                                                           .ToDiv("Test")
                                                           .ToHtmlString()
                                                           .ShouldEqual("<div>Test</div>");

        It should_be_to_i = () => new RouteValueDictionary(new { @class = "class" })
                                          .ToI()
                                          .ToHtmlString()
                                          .ShouldEqual("<i class=\"class\"></i>");

        It should_be_to_i_with_content = () => new RouteValueDictionary(new { @class = "class" })
                                                       .ToI("Test")
                                                       .ToHtmlString()
                                                       .ShouldEqual("<i class=\"class\">Test</i>");

        It should_be_to_i_with_content_as_func = () => new RouteValueDictionary(new { @class = "class" })
                                                               .ToI(o => new HelperResult(writer => writer.WriteLine("<div>")))
                                                               .ToHtmlString()
                                                               .ShouldEqual("<i class=\"class\"><div></i>"); //-V3050

        It should_be_to_img = () => new RouteValueDictionary(new { @class = "class" })
                                            .ToImg()
                                            .ToHtmlString()
                                            .ShouldEqual("<img class=\"class\"></img>");

        It should_be_to_img_with_content = () => new RouteValueDictionary(new { @class = "class" })
                                                         .ToImg("Test")
                                                         .ToHtmlString()
                                                         .ShouldEqual("<img class=\"class\">Test</img>");

        It should_be_to_img_with_content_as_func = () => new RouteValueDictionary(new { @class = "class" })
                                                                 .ToImg(o => new HelperResult(writer => writer.WriteLine("<div>")))
                                                                 .ToHtmlString()
                                                                 .ShouldEqual("<img class=\"class\"><div></img>"); //-V3050

        It should_be_to_label = () => new RouteValueDictionary(new { @class = "class" })
                                              .ToLabel()
                                              .ToHtmlString()
                                              .ShouldEqual("<label class=\"class\"></label>");

        It should_be_to_label_with_content = () => new RouteValueDictionary(new { @class = "class" })
                                                           .ToLabel("Test")
                                                           .ToHtmlString()
                                                           .ShouldEqual("<label class=\"class\">Test</label>");

        It should_be_to_label_with_content_as_func = () => new RouteValueDictionary(new { @class = "class" })
                                                                   .ToLabel(o => new HelperResult(writer => writer.WriteLine("<div>")))
                                                                   .ToHtmlString()
                                                                   .ShouldEqual("<label class=\"class\"><div></label>"); //-V3050

        It should_be_to_link = () => new RouteValueDictionary(new { @class = "class" })
                                             .ToLink()
                                             .ToHtmlString()
                                             .ShouldEqual("<a class=\"class\" href=\"javascript:void(0);\"></a>");

        It should_be_to_link_replace_href = () => new RouteValueDictionary(new { @href = "http://asdsd" })
                                                          .ToLink("Value")
                                                          .ToHtmlString()
                                                          .ShouldEqual("<a href=\"http://asdsd\">Value</a>");

        It should_be_to_link_with_content = () => new RouteValueDictionary(new { @class = "class" })
                                                          .ToLink("Value")
                                                          .ToHtmlString()
                                                          .ShouldEqual("<a class=\"class\" href=\"javascript:void(0);\">Value</a>");

        It should_be_to_link_with_content_as_helper = () => new RouteValueDictionary(new { @class = "class" })
                                                                    .ToLink(o => new HelperResult(writer => writer.WriteLine("<div>")))
                                                                    .ToHtmlString()
                                                                    .ShouldEqual("<a class=\"class\" href=\"javascript:void(0);\"><div></a>"); //-V3050

        It should_be_to_radio_button = () => new RouteValueDictionary(new { @class = "class" })
                                                     .ToRadioButton("value", false)
                                                     .ToHtmlString()
                                                     .ShouldEqual("<input class=\"class\" type=\"radio\" value=\"value\" />");

        It should_be_to_radio_button_checked = () => new RouteValueDictionary(new { @class = "class" })
                                                             .ToRadioButton("value", true)
                                                             .ToHtmlString()
                                                             .ShouldEqual("<input checked=\"checked\" class=\"class\" type=\"radio\" value=\"value\" />");

        It should_be_to_select = () => new RouteValueDictionary(new { @class = "class" })
                                               .ToSelect()
                                               .ToHtmlString()
                                               .ShouldEqual("<select class=\"class\"></select>");

        It should_be_to_submit = () => new RouteValueDictionary(new { @class = "class" })
                                               .ToSubmit("Value")
                                               .ToHtmlString()
                                               .ShouldEqual("<input class=\"class\" type=\"submit\" value=\"Value\" />");

        It should_be_to_tag = () => new RouteValueDictionary(new { @class = "class" })
                                            .ToTag(HtmlTag.Label)
                                            .ToHtmlString()
                                            .ShouldEqual("<label class=\"class\"></label>");

        It should_be_to_tag_with_content = () => new RouteValueDictionary(new { @class = "class" })
                                                         .ToTag(HtmlTag.Label, "Test")
                                                         .ToHtmlString()
                                                         .ShouldEqual("<label class=\"class\">Test</label>");

        It should_be_to_tag_with_content_as_func = () => new RouteValueDictionary(new { @class = "class" })
                                                                 .ToTag(HtmlTag.Label, o => new HelperResult(writer => writer.WriteLine("<div>")))
                                                                 .ToHtmlString()
                                                                 .ShouldEqual("<label class=\"class\"><div></label>"); //-V3050

        It should_be_to_text_area = () => new RouteValueDictionary(new { @class = "class" })
                                                  .ToTextArea()
                                                  .ToHtmlString()
                                                  .ShouldEqual("<textarea class=\"class\"></textarea>");

        It should_be_to_text_box = () => new RouteValueDictionary(new { @class = "class" })
                                                 .ToTextBox("Value")
                                                 .ToHtmlString()
                                                 .ShouldEqual("<input class=\"class\" type=\"text\" value=\"Value\" />");
    }
}