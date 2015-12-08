namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(Selector))]
    public class When_jquery_selector_issue
    {
        It should_be_121 = () => Selector.Jquery.Class("nestedInputList").Tag(HtmlTag.Input)
                                         .ToString()
                                         .ShouldEqual("$('.nestedInputList input')");

        It should_be_124 = () => Selector.Jquery.Name("SelectedDropOutType")
                                         .Tag(HtmlTag.Option)
                                         .Expression(JqueryExpression.Selected)
                                         .ToString()
                                         .ShouldEqual("$('[name=\"SelectedDropOutType\"] option:selected')");

        It should_be_127_aka_find = () => Selector.Jquery.Class("navigation")
                                                  .Tag(HtmlTag.Ul)
                                                  .Tag(HtmlTag.Li)
                                                  .ToString()
                                                  .ShouldEqual("$('.navigation ul li')");

        It should_be_127_immediate = () => Selector.Jquery.Class("navigation")
                                                   .Immediate()
                                                   .Tag(HtmlTag.Ul)
                                                   .Immediate()
                                                   .Tag(HtmlTag.Li)
                                                   .ToString()
                                                   .ShouldEqual("$('.navigation > ul > li')");

        It should_be_131 = () => Selector.Jquery.Id("quickFactsTabs")
                                         .Tag(HtmlTag.Li).Eq(0)
                                         .ToString()
                                         .ShouldEqual("$('#quickFactsTabs li:eq(0)')");

        It should_be_205 = () => Selector.Jquery.Class("nestedInputList")
                                         .Find(HtmlTag.Input)
                                         .ToString()
                                         .ShouldEqual("$('.nestedInputList').find('input')");

        It should_be_226_closest_in_closest = () => Selector.Jquery.Name("IndicatorIds")
                                                            .Closest(r => r.Class("control-group").Closest(HtmlTag.Tr))
                                                            .ToString()
                                                            .ShouldEqual("$('[name=\"IndicatorIds\"]').closest($('.control-group').closest('tr'))");

        It should_be_330_closure_selector = () =>
                                            {
                                                foreach (var pair in new Dictionary<Func<JquerySelectorExtend, JquerySelectorExtend>, string>
                                                                     {
                                                                             { selector => selector.Attr(HtmlAttribute.Value), "attr('value')" },
                                                                             { selector => selector.Attr("value"), "attr('value')" },
                                                                             { selector => selector.Css(CssStyling.Width), "css('width')" },
                                                                             { selector => selector.Is(JqueryExpression.Submit), "is($(':submit'))" },
                                                                             { selector => selector.Is(jquerySelector => jquerySelector.Class("test")), "is($('.test'))" },
                                                                             { selector => selector.Width(), "width()" },
                                                                             { selector => selector.Height(), "height()" },
                                                                     })
                                                {
                                                    var self = Selector.Jquery.Self();
                                                    Pleasure.Do3(i => { pair.Key(self).ToString().ShouldEqual("$(this.self)." + pair.Value); });
                                                }
                                            };

        It should_be_98 = () => Selector.Jquery.Class("extraCheckbox")
                                        .Not(selector => selector.Expression(JqueryExpression.Checked))
                                        .ToString()
                                        .ShouldEqual("$('.extraCheckbox').not(':checked')");

        It should_be_expression_after_method = () => Selector.Jquery.Self()
                                                             .Find(HtmlTag.Label)
                                                             .Expression(JqueryExpression.First)
                                                             .ToString()
                                                             .ShouldEqual("$(this.self).find('label').filter(':first')");

        It should_be_inc_404_find_in_find = () => Selector.Jquery.Id("id").Find(r => r.Tag(HtmlTag.Td).Eq(4).Find(HtmlTag.Input))
                                                          .ToString()
                                                          .ShouldEqual("$('#id').find($('td:eq(4)').find('input'))");

        It should_be_inc_425_complexity = () => Selector.Jquery.Self().Closest(HtmlTag.TBody)
                                                        .Filter(HtmlTag.Tr)
                                                        .Not(selector => selector.Expression(JqueryExpression.First))
                                                        .ToString()
                                                        .ShouldEqual("$(this.self).closest('tbody').filter('tr').not(':first')");

        It should_be_inc_425_complexity_2 = () => Selector.Jquery.Self().Closest(HtmlTag.TBody)
                                                          .Filter(HtmlTag.Tr)
                                                          .Expression(JqueryExpression.Button)
                                                          .ToString()
                                                          .ShouldEqual("$(this.self).closest('tbody').filter('tr').filter(':button')");

        It should_be_self_with_attr = () => Selector.Jquery.Self()
                                                    .Attr("units")
                                                    .ToString()
                                                    .ShouldEqual("$(this.self).attr('units')");

        It should_be_tag_also_attr = () => Selector.Jquery.Tag(HtmlTag.Input)
                                                   .EqualsAttribute(HtmlAttribute.Type, "text")
                                                   .ToString()
                                                   .ShouldEqual("$('input[type=\"text\"]')");

        It should_be_tag_also_attr_by_at_once = () => Selector.Jquery.Tag(HtmlTag.Input)
                                                              .EqualsAttribute(HtmlAttribute.Multiple)
                                                              .ToString()
                                                              .ShouldEqual("$('input[multiple=\"multiple\"]')");
        
    }
}