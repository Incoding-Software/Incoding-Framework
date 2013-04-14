namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JquerySelector))]
    public class When_jquery_selector
    {
        #region Fake classes

        class FakeForm
        {
            #region Properties

            public FakeInput Input { get; set; }

            public decimal Prop { get; set; }

            #endregion

            #region Nested classes

            internal class FakeInput
            {
                #region Properties

                public string Id { get; set; }

                public Guid? HealthCareSystemId { get; set; }

                public long Long { get; set; }

                public long? NullableLong { get; set; }

                public long[] ArrayIds { get; set; }

                #endregion
            }

            #endregion
        }

        #endregion

        It should_be_to_all = () => Selector.Jquery
                                            .All()
                                            .ToString()
                                            .ShouldEqual("$('*')");

        It should_be_to_custom = () => Selector.Jquery
                                               .Custom("#id id")
                                               .ToString()
                                               .ShouldEqual("$('#id id')");

        #region Id

        It should_be_to_id = () => Selector.Jquery
                                           .Id("id")
                                           .ToString().ShouldEqual("$('#id')");

        It should_be_to_id_expression = () => Selector.Jquery
                                                      .Id<FakeForm>(r => r.Prop)
                                                      .ToString().ShouldEqual("$('#Prop')");

        It should_be_to_id_nullable_expression = () => Selector.Jquery
                                                               .Id<FakeForm>(r => r.Input.HealthCareSystemId)
                                                               .ToString().ShouldEqual("$('#Input_HealthCareSystemId')");

        It should_be_to_id_long_expression = () => Selector.Jquery
                                                           .Id<FakeForm>(r => r.Input.Long)
                                                           .ToString().ShouldEqual("$('#Input_Long')");

        It should_be_to_id_nullable_long_expression = () => Selector.Jquery
                                                                    .Id<FakeForm>(r => r.Input.NullableLong)
                                                                    .ToString().ShouldEqual("$('#Input_NullableLong')");

        It should_be_to_id_expression_with_array = () => Selector.Jquery
                                                                 .Id<FakeForm>(r => r.Input.ArrayIds[0]).ToString()
                                                                 .ShouldEqual("$('#Input_ArrayIds[0]')");

        #endregion

        #region Name

        It should_be_to_name = () => Selector.Jquery
                                             .Name("CountryId").ToString()
                                             .ShouldEqual("$('[name=\"CountryId\"]')");

        It should_be_to_name_expression = () => Selector.Jquery
                                                        .Name<FakeForm>(r => r.Prop).ToString()
                                                        .ShouldEqual("$('[name=\"Prop\"]')");

        It should_be_to_name_nullable_expression = () => Selector.Jquery
                                                                 .Name<FakeForm>(r => r.Input.HealthCareSystemId).ToString()
                                                                 .ShouldEqual("$('[name=\"Input.HealthCareSystemId\"]')");

        It should_be_to_name_long_expression = () => Selector.Jquery
                                                             .Name<FakeForm>(r => r.Input.Long).ToString()
                                                             .ShouldEqual("$('[name=\"Input.Long\"]')");

        It should_be_to_name_nullable_long_expression = () => Selector.Jquery
                                                                      .Name<FakeForm>(r => r.Input.NullableLong)
                                                                      .ToString().ShouldEqual("$('[name=\"Input.NullableLong\"]')");

        It should_be_to_name_expression_with_array = () => Selector.Jquery
                                                                   .Name<FakeForm>(r => r.Input.ArrayIds[0]).ToString()
                                                                   .ShouldEqual("$('[name=\"Input.ArrayIds[0]\"]')");

        #endregion

        It should_be_to_class = () => Selector.Jquery
                                              .Class("class")
                                              .ToString()
                                              .ShouldEqual("$('.class')");

        It should_be_to_class_with_escaping = () => Selector.Jquery
                                                            .Class(".class")
                                                            .ToString()
                                                            .ShouldEqual("$('.\\.class')");

        It should_be_to_self = () => Selector.Jquery
                                             .Self()
                                             .ToString()
                                             .ShouldEqual("$(this.self)");

        It should_be_to_target = () => Selector.Jquery
                                               .Target()
                                               .ToString()
                                               .ShouldEqual("$(this.target)");

        It should_be_to_document = () => Selector.Jquery
                                                 .Document()
                                                 .ToString()
                                                 .ShouldEqual("$(window.document)");

        It should_be_to_dom = () => Selector.Jquery
                                            .Tag(HtmlTag.Div)
                                            .ToString()
                                            .ShouldEqual("$('div')");

        It should_be_visible = () => Selector.Jquery
                                             .Tag(HtmlTag.Div)
                                             .Visible()
                                             .ToString()
                                             .ShouldEqual("$('div:visible')");

        It should_be_last = () => Selector.Jquery
                                          .Tag(HtmlTag.Div)
                                          .Last()
                                          .ToString()
                                          .ShouldEqual("$('div:last')");

        It should_be_expression = () => Selector.Jquery
                                                .Tag(HtmlTag.Div)
                                                .Expression(JqueryExpression.Last_Child)
                                                .ToString()
                                                .ShouldEqual("$('div:last_child')");

        It should_be_to_contains_attribute = () => Selector.Jquery
                                                           .ContainsAttribute(HtmlAttribute.Type, InputType.Text.ToString())
                                                           .ToString()
                                                           .ShouldEqual("$('[type*=\"Text\"]')");

        It should_be_to_equals_attribute = () => Selector.Jquery
                                                         .EqualsAttribute(HtmlAttribute.Type, InputType.Text.ToString())
                                                         .ToString()
                                                         .ShouldEqual("$('[type=\"Text\"]')");

        It should_be_to_equals_attribute_with_escaping = () => Selector.Jquery
                                                                       .EqualsAttribute(HtmlAttribute.Type, "input.MyValue")
                                                                       .ToString()
                                                                       .ShouldEqual("$('[type=\"input.MyValue\"]')");

        It should_be_to_not_equals_attribute = () => Selector.Jquery
                                                             .NotEqualsAttribute(HtmlAttribute.Type, InputType.Text.ToString())
                                                             .ToString()
                                                             .ShouldEqual("$('[type!=\"Text\"]')");

        It should_be_to_ends_with_attribute = () => Selector.Jquery
                                                            .EndsWithAttribute(HtmlAttribute.Type, InputType.Text.ToString())
                                                            .ToString()
                                                            .ShouldEqual("$('[type$=\"Text\"]')");

        It should_be_to_start_with_attribute = () => Selector.Jquery
                                                             .StartWithAttribute(HtmlAttribute.Type, InputType.Text.ToString())
                                                             .ToString()
                                                             .ShouldEqual("$('[type^=\"Text\"]')");

        It should_be_to_has_attribute = () => Selector.Jquery
                                                      .HasAttribute(HtmlAttribute.Name)
                                                      .ToString().ShouldEqual("$('[name]')");

        It should_be_custom = () => Selector.Jquery
                                            .Custom("myPlugIn()")
                                            .ToString()
                                            .ShouldEqual("$('myPlugIn()')");

        It should_be_ids = () => Selector.Jquery
                                         .Id("id", "newId")
                                         .ToString()
                                         .ShouldEqual("$('#id,#newId')");

        It should_be_classes = () => Selector.Jquery
                                             .Class("id", "class")
                                             .ToString()
                                             .ShouldEqual("$('.id,.class')");

        It should_be_it = () => Selector.Jquery.All()
                                        .It(5).ToString()
                                        .ShouldEqual("$('*:it(5)')");

        It should_be_gt = () => Selector.Jquery.All()
                                        .Gt(5)
                                        .ToString()
                                        .ShouldEqual("$('*:gt(5)')");

        It should_be_eq = () => Selector.Jquery.All()
                                        .Eq(5)
                                        .ToString()
                                        .ShouldEqual("$('*:eq(5)')");

        It should_be_has = () => Selector.Jquery.All()
                                         .Has(selector => selector.Tag(HtmlTag.P))
                                         .ToString()
                                         .ShouldEqual("$('*:has(p)')");

        It should_be_not = () => Selector.Jquery.All()
                                         .Not(selector => selector.Tag(HtmlTag.P))
                                         .ToString()
                                         .ShouldEqual("$('*:not(p)')");

        It should_be_id_tag_gt = () => Selector.Jquery
                                               .Id("id")
                                               .Tag(HtmlTag.Div)
                                               .Gt(10)
                                               .ToString()
                                               .ShouldEqual("$('#id div:gt(10)')");

        It should_be_also = () => Selector.Jquery
                                          .Id("id")
                                          .Or(r => r.Id("nextId"))
                                          .ToString()
                                          .ShouldEqual("$('#id,#nextId')");

        It should_be_or = () => Selector.Jquery
                                        .Class("first")
                                        .Also(r => r.Class("next"))
                                        .ToString()
                                        .ShouldEqual("$('.first.next')");
    }
}