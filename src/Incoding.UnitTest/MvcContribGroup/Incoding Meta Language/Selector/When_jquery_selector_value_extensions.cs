namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JquerySelectorExtendValueExtensions))]
    public class When_jquery_selector_value_extensions
    {
        It should_be_text = () => Selector.Jquery
                                          .Self()
                                          .Text()
                                          .ToString()
                                          .ShouldEqual("$(this.self).text()");

        It should_be_height = () => Selector.Jquery
                                            .Self()
                                            .Height()
                                            .ToString()
                                            .ShouldEqual("$(this.self).height()");

        It should_be_length = () => Selector.Jquery
                                            .Self()
                                            .Length()
                                            .ToString()
                                            .ShouldEqual("$(this.self).length");

        It should_be_val = () => Selector.Jquery
                                         .Self()
                                         .Val()
                                         .ToString()
                                         .ShouldEqual("$(this.self).val()");

        It should_be_inner_height = () => Selector.Jquery
                                                  .Self()
                                                  .InnerHeight()
                                                  .ToString()
                                                  .ShouldEqual("$(this.self).innerHeight()");

        It should_be_has_class = () => Selector.Jquery
                                               .Self()
                                               .HasClass("class")
                                               .ToString()
                                               .ShouldEqual("$(this.self).hasClass('class')");

        It should_be_has_class_multiple = () => Selector.Jquery
                                                        .Self()
                                                        .HasClass("class class2")
                                                        .ToString()
                                                        .ShouldEqual("$(this.self).hasClass('class class2')");

        It should_be_form_is_valid = () => Selector.Jquery
                                                   .Self()
                                                   .FormIsValid()
                                                   .ToString()
                                                   .ShouldEqual("$(this.self).valid()");

        It should_be_is = () => Selector.Jquery
                                        .Self()
                                        .Is(Selector.Jquery.Self())
                                        .ToString()
                                        .ShouldEqual("$(this.self).is($(this.self))");

        It should_be_is_tag = () => Selector.Jquery
                                            .Self()
                                            .Is(HtmlTag.Hr)
                                            .ToString()
                                            .ShouldEqual("$(this.self).is($('hr'))");

        It should_be_is_expression = () => Selector.Jquery
                                                   .Self()
                                                   .Is(JqueryExpression.Checkbox)
                                                   .ToString()
                                                   .ShouldEqual("$(this.self).is($(':checkbox'))");

        It should_be_is_evaluated = () => Selector.Jquery
                                                  .Self()
                                                  .Is(selector => selector.Id("id"))
                                                  .ToString()
                                                  .ShouldEqual("$(this.self).is($('#id'))");

        It should_be_attr = () => Selector.Jquery
                                          .Self()
                                          .Attr(HtmlAttribute.Alt)
                                          .ToString()
                                          .ShouldEqual("$(this.self).attr('alt')");

        It should_be_css = () => Selector.Jquery
                                         .Self()
                                         .Css(CssStyling.FontSize)
                                         .ToString()
                                         .ShouldEqual("$(this.self).css('font-size')");

        It should_be_property = () => Selector.Jquery
                                              .Self()
                                              .Property("length")
                                              .ToString()
                                              .ShouldEqual("$(this.self).length");

        It should_be_method = () => Selector.Jquery
                                            .Self()
                                            .Method("myMethod")
                                            .ToString()
                                            .ShouldEqual("$(this.self).myMethod()");

        It should_be_method_with_args = () => Selector.Jquery
                                                      .Self()
                                                      .Method("myMethod", "aws")
                                                      .ToString()
                                                      .ShouldEqual("$(this.self).myMethod('aws')");

        It should_be_method_with_args_selector = () => Selector.Jquery
                                                               .Self()
                                                               .Method("myMethod", Selector.Jquery.Id("aws")).ToString()
                                                               .ShouldEqual("$(this.self).myMethod($('#aws'))");

        It should_be_inner_width = () => Selector.Jquery
                                                 .Self()
                                                 .InnerWidth()
                                                 .ToString()
                                                 .ShouldEqual("$(this.self).innerWidth()");

        It should_be_outer_height = () => Selector.Jquery
                                                  .Self()
                                                  .OuterHeight()
                                                  .ToString()
                                                  .ShouldEqual("$(this.self).outerHeight()");

        It should_be_outer_width = () => Selector.Jquery
                                                 .Self()
                                                 .OuterWidth()
                                                 .ToString()
                                                 .ShouldEqual("$(this.self).outerWidth()");

        It should_be_scroll_left = () => Selector.Jquery
                                                 .Self()
                                                 .ScrollLeft().ToString()
                                                 .ShouldEqual("$(this.self).scrollLeft()");

        It should_be_scroll_top = () => Selector.Jquery
                                                .Self()
                                                .ScrollTop()
                                                .ToString()
                                                .ShouldEqual("$(this.self).scrollTop()");

        It should_be_width = () => Selector.Jquery
                                           .Self()
                                           .Width()
                                           .ToString()
                                           .ShouldEqual("$(this.self).width()");
    }
}