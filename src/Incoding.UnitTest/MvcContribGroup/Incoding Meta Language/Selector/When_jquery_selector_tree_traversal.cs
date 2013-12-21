namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JquerySelectorExtendTreeTraversalExtensions))]
    public class When_jquery_selector_tree_traversal
    {
        It should_be_children_without_selector = () => Selector.Jquery
                                                               .Self()
                                                               .Children(HtmlTag.A).ToString()
                                                               .ShouldEqual("$(this.self).children('a')");

        It should_be_children = () => Selector.Jquery
                                              .Self()
                                              .Children(selector => selector.Tag(HtmlTag.Area)).ToString()
                                              .ShouldEqual("$(this.self).children('area')");   
        
        It should_be_children_with_null = () => Selector.Jquery
                                              .Self()
                                              .Children().ToString()
                                              .ShouldEqual("$(this.self).children()");

        It should_be_closest = () => Selector.Jquery
                                             .Self()
                                             .Closest(selector => selector.Tag(HtmlTag.Area)).ToString()
                                             .ShouldEqual("$(this.self).closest('area')");

        It should_be_closest_without_selector = () => Selector.Jquery
                                                              .Self()
                                                              .Closest(HtmlTag.Abbr).ToString()
                                                              .ShouldEqual("$(this.self).closest('abbr')");

        It should_be_find = () => Selector.Jquery
                                          .Self()
                                          .Find(selector => selector.Tag(HtmlTag.Area)).ToString()
                                          .ShouldEqual("$(this.self).find('area')");

        It should_be_find_without_selector = () => Selector.Jquery
                                                           .Self()
                                                           .Find(HtmlTag.Area).ToString()
                                                           .ShouldEqual("$(this.self).find('area')");

        It should_be_filter = () => Selector.Jquery
                                            .Self()
                                            .Filter(selector => selector.Tag(HtmlTag.Area)).ToString()
                                            .ShouldEqual("$(this.self).filter('area')");

        It should_be_filter_without_selector = () => Selector.Jquery
                                                             .Self()
                                                             .Filter(HtmlTag.Article).ToString()
                                                             .ShouldEqual("$(this.self).filter('article')");

        It should_be_next = () => Selector.Jquery
                                          .Self()
                                          .Next(selector => selector.Tag(HtmlTag.Area)).ToString()
                                          .ShouldEqual("$(this.self).next('area')");

        It should_be_next_without_selector = () => Selector.Jquery
                                                           .Self()
                                                           .Next().ToString()
                                                           .ShouldEqual("$(this.self).next()");

        It should_be_next_all = () => Selector.Jquery
                                              .Self()
                                              .NextAll(selector => selector.Tag(HtmlTag.Area)).ToString()
                                              .ShouldEqual("$(this.self).nextAll('area')");

        It should_be_next_all_without_selector = () => Selector.Jquery
                                                               .Self()
                                                               .NextAll().ToString()
                                                               .ShouldEqual("$(this.self).nextAll()");

        It should_be_next_until = () => Selector.Jquery
                                                .Self()
                                                .NextUntil(selector => selector.Tag(HtmlTag.Area)).ToString()
                                                .ShouldEqual("$(this.self).nextUntil('area')");

        It should_be_next_until_without_selector = () => Selector.Jquery
                                                                 .Self()
                                                                 .NextUntil().ToString()
                                                                 .ShouldEqual("$(this.self).nextUntil()");

        It should_be_offset_parent = () => Selector.Jquery
                                                   .Self()
                                                   .OffsetParent().ToString()
                                                   .ShouldEqual("$(this.self).offsetParent()");

        It should_be_parent = () => Selector.Jquery
                                            .Self()
                                            .Parent(selector => selector.Tag(HtmlTag.Area)).ToString()
                                            .ShouldEqual("$(this.self).parent('area')");

        It should_be_parent_without_selector = () => Selector.Jquery
                                                             .Self()
                                                             .Parent()
                                                             .ToString()
                                                             .ShouldEqual("$(this.self).parent()");

        It should_be_parents = () => Selector.Jquery
                                             .Self()
                                             .Parents(HtmlTag.Area).ToString()
                                             .ShouldEqual("$(this.self).parents('area')");

        It should_be_parents_without_selector = () => Selector.Jquery
                                                              .Self()
                                                              .Parents(selector => selector.Tag(HtmlTag.Area)).ToString()
                                                              .ShouldEqual("$(this.self).parents('area')");

        It should_be_parents_until = () => Selector.Jquery
                                                   .Self()
                                                   .ParentsUntil().ToString()
                                                   .ShouldEqual("$(this.self).parentsUntil()");

        It should_be_prev = () => Selector.Jquery
                                          .Self()
                                          .Prev(selector => selector.Tag(HtmlTag.Area)).ToString()
                                          .ShouldEqual("$(this.self).prev('area')");

        It should_be_prev_without_selector = () => Selector.Jquery
                                                           .Self()
                                                           .Prev().ToString()
                                                           .ShouldEqual("$(this.self).prev()");

        It should_be_prev_all = () => Selector.Jquery
                                              .Self()
                                              .PrevAll(selector => selector.Tag(HtmlTag.Area)).ToString()
                                              .ShouldEqual("$(this.self).prevAll('area')");

        It should_be_prev_all_without_selector = () => Selector.Jquery
                                                               .Self()
                                                               .PrevAll().ToString()
                                                               .ShouldEqual("$(this.self).prevAll()");

        It should_be_prev_until = () => Selector.Jquery
                                                .Self()
                                                .PrevUntil(selector => selector.Tag(HtmlTag.Area)).ToString()
                                                .ShouldEqual("$(this.self).prevUntil('area')");

        It should_be_prev_until_without_selector = () => Selector.Jquery
                                                                 .Self()
                                                                 .PrevUntil().ToString()
                                                                 .ShouldEqual("$(this.self).prevUntil()");

        It should_be_siblings = () => Selector.Jquery
                                              .Self()
                                              .Siblings(selector => selector.Tag(HtmlTag.Area)).ToString()
                                              .ShouldEqual("$(this.self).siblings('area')");

        It should_be_siblings_without_selector = () => Selector.Jquery
                                                               .Self()
                                                               .Siblings().ToString()
                                                               .ShouldEqual("$(this.self).siblings()");

        It should_be_add = () => Selector.Jquery
                                         .Self()
                                         .Add(selector => selector.Tag(HtmlTag.Area)).ToString()
                                         .ShouldEqual("$(this.self).add('area')");

        It should_be_multiple_tree = () => Selector.Jquery
                                                   .Self()
                                                   .Add(selector => selector.Tag(HtmlTag.Area))
                                                   .Parents()
                                                   .Closest(selector => selector.Tag(HtmlTag.Tr))
                                                   .ToString()
                                                   .ShouldEqual("$(this.self).add('area').parents().closest('tr')");
    }
}