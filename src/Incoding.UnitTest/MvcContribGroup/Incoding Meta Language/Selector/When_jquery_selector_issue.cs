namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(Selector))]
    public class When_jquery_selector_issue
    {
        It should_be_98 = () => Selector.Jquery.Class("extraCheckbox")
                                        .Not(selector => selector.Expression(JqueryExpression.Checked))
                                        .ToString()
                                        .ShouldEqual("$('.extraCheckbox:not(:checked)')");

        It should_be_205 = () => Selector.Jquery.Class("nestedInputList")
                                         .Find(HtmlTag.Input)
                                         .ToString()
                                         .ShouldEqual("$('.nestedInputList').find($('input'))");

        It should_be_121 = () => Selector.Jquery.Class("nestedInputList").Tag(HtmlTag.Input)
                                         .ToString()
                                         .ShouldEqual("$('.nestedInputList input')");

        It should_be_131 = () => Selector.Jquery.Id("quickFactsTabs")
                                         .Tag(HtmlTag.Li).Eq(0)
                                         .ToString()
                                         .ShouldEqual("$('#quickFactsTabs li:eq(0)')");

        It should_be_127_immediate = () => Selector.Jquery.Class("navigation")
                                                   .Immediate()
                                                   .Tag(HtmlTag.Ul)
                                                   .Immediate()
                                                   .Tag(HtmlTag.Li)
                                                   .ToString()
                                                   .ShouldEqual("$('.navigation > ul > li')");

        It should_be_127_aka_find = () => Selector.Jquery.Class("navigation")
                                                  .Tag(HtmlTag.Ul)
                                                  .Tag(HtmlTag.Li)
                                                  .ToString()
                                                  .ShouldEqual("$('.navigation ul li')");

        It should_be_124 = () => Selector.Jquery.Name("SelectedDropOutType")
                                         .Tag(HtmlTag.Option)
                                         .Expression(JqueryExpression.Selected)
                                         .ToString()
                                         .ShouldEqual("$('[name=\"SelectedDropOutType\"] option:selected')");

        It should_be_tag_also_attr = () => Selector.Jquery.Tag(HtmlTag.Input)
                                                   .EqualsAttribute(HtmlAttribute.Type, "text")
                                                   .ToString()
                                                   .ShouldEqual("$('input[type=\"text\"]')");

        It should_be_226_closest_in_closest = () => Selector.Jquery.Name("IndicatorIds")
                                                            .Closest(r => r.Class("control-group").Closest(HtmlTag.Tr))
                                                            .ToString()
                                                            .ShouldEqual("$('[name=\"IndicatorIds\"]').closest($('.control-group').closest($('tr')))");

        It should_be_330_closure_selector = () =>
                                                {
                                                    var self = Selector.Jquery.Self();
                                                    Pleasure.Do3(i => self.Length()
                                                                          .ToString()
                                                                          .ShouldEqual("$(this.self).length"));
                                                };

        It should_be_expression_after_method = () => Selector.Jquery.Self()
                                                             .Find(HtmlTag.Label)
                                                             .Expression(JqueryExpression.First)
                                                             .ToString()
                                                             .ShouldEqual("$(this.self).find('label').filter(':first')");
    }
}