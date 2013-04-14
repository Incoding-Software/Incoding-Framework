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
                                          .Text().ToString()
                                          .ShouldEqual("$(this.self).text()");

        It should_be_height = () => Selector.Jquery
                                            .Self()
                                            .Height().ToString()
                                            .ShouldEqual("$(this.self).height()");

        It should_be_inner_height = () => Selector.Jquery
                                                  .Self()
                                                  .InnerHeight().ToString()
                                                  .ShouldEqual("$(this.self).innerHeight()");

        It should_be_inner_width = () => Selector.Jquery
                                                 .Self()
                                                 .InnerWidth().ToString()
                                                 .ShouldEqual("$(this.self).innerWidth()");

        It should_be_outer_height = () => Selector.Jquery
                                                  .Self()
                                                  .OuterHeight().ToString()
                                                  .ShouldEqual("$(this.self).outerHeight()");

        It should_be_outer_width = () => Selector.Jquery
                                                 .Self()
                                                 .OuterWidth().ToString()
                                                 .ShouldEqual("$(this.self).outerWidth()");

        It should_be_scroll_left = () => Selector.Jquery
                                                 .Self()
                                                 .ScrollLeft().ToString()
                                                 .ShouldEqual("$(this.self).scrollLeft()");

        It should_be_scroll_top = () => Selector.Jquery
                                                .Self()
                                                .ScrollTop().ToString()
                                                .ShouldEqual("$(this.self).scrollTop()");

        It should_be_width = () => Selector.Jquery
                                           .Self()
                                           .Width().ToString()
                                           .ShouldEqual("$(this.self).width()");
    }
}