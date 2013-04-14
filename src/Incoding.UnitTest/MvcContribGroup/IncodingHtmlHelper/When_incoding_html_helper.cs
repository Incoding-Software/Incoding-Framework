namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncodingHtmlHelper))]
    public class When_incoding_html_helper
    {
        #region Static Fields

        protected static IncodingHtmlHelper incodingHtml;

        #endregion

        Establish establish = () => { incodingHtml = new IncodingHtmlHelper(MockHtmlHelper<object>.When().Original); };

        It should_be_button = () => incodingHtml.Button("Add", new { id = "Id", @class = "test" })
                                                .ToHtmlString()
                                                .ShouldEqual("<button class=\"test\" id=\"Id\">Add</button>");

        It should_be_script = () => incodingHtml
                                            .Script("Controller/Action")
                                            .ToHtmlString()
                                            .ShouldEqual("<script src=\"Controller/Action\" type=\"text/javascript\"></script>");

        It should_be_css = () => incodingHtml
                                         .Css("Controller/Action")
                                         .ToHtmlString()
                                         .ShouldEqual("<link href=\"Controller/Action\" rel=\"stylesheet\" type=\"text/css\"></link>");

        It should_be_template = () => incodingHtml
                                              .Template<When_incoding_html_helper>()
                                              .Should(template => template.TryGetValue("htmlHelper").ShouldNotBeNull());

        It should_be_begin_tag = () => incodingHtml
                                               .BeginTag(HtmlTag.Area, new { @class = "class" })
                                               .Should(beginTag =>
                                                           {
                                                               beginTag.TryGetValue("htmlHelper").ShouldNotBeNull();
                                                               beginTag.TryGetValue("tag").ShouldEqual("area");
                                                           });

        It should_be_script_template = () => incodingHtml
                                                     .ScriptTemplate<When_incoding_html_helper>(Pleasure.Generator.TheSameString())
                                                     .Should(template => template.TryGetValue("htmlHelper").ShouldNotBeNull());

        It should_be_submit = () => incodingHtml
                                            .Submit("Value", new { @class = "class" })
                                            .ToHtmlString()
                                            .ShouldEqual("<input class=\"class\" type=\"submit\" value=\"Value\" />");

        It should_be_file = () => incodingHtml
                                          .File("Value", new { @class = "class" })
                                          .ToHtmlString()
                                          .ShouldEqual("<input class=\"class\" type=\"file\" value=\"Value\" />");

        It should_be_img = () => incodingHtml.Img("Controller/Action", new { @class = "class" })
                                             .ToHtmlString()
                                             .ShouldEqual("<img class=\"class\" src=\"Controller/Action\"></img>");

        It should_be_div = () => incodingHtml
                                         .Div("<p></p>", new { @class = "class" })
                                         .ToHtmlString()
                                         .ShouldEqual("<div class=\"class\"><p></p></div>");

        It should_be_span = () => incodingHtml
                                          .Span("<p></p>", new { @class = "class" })
                                          .ToHtmlString()
                                          .ShouldEqual("<span class=\"class\"><p></p></span>");

        It should_be_i = () => incodingHtml
                                       .I(new { @class = "class" })
                                       .ToHtmlString()
                                       .ShouldEqual("<i class=\"class\"></i>");

        It should_be_p = () => incodingHtml
                                       .P("<p></p>", new { @class = "class" })
                                       .ToHtmlString()
                                       .ShouldEqual("<p class=\"class\"><p></p></p>");

        It should_be_ul = () => incodingHtml
                                        .Ul("<p></p>", new { @class = "class" })
                                        .ToHtmlString()
                                        .ShouldEqual("<ul class=\"class\"><p></p></ul>");

        It should_be_li = () => incodingHtml
                                        .Li("<p></p>", new { @class = "class" })
                                        .ToHtmlString()
                                        .ShouldEqual("<li class=\"class\"><p></p></li>");

        It should_be_tag = () => incodingHtml
                                         .Tag(HtmlTag.Button, "<p></p>", new { @class = "class" })
                                         .ToHtmlString()
                                         .ShouldEqual("<button class=\"class\"><p></p></button>");

        It should_be_link = () => incodingHtml
                                          .Link("url", "Name", new { @class = "class" })
                                          .ToHtmlString()
                                          .ShouldEqual("<a class=\"class\" href=\"url\">Name</a>");
    }
}