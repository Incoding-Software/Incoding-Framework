namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ConditionalIs))]
    public class When_conditional_is
    {
        #region Fake classes

        class FakeModel
        {
            #region Properties

            public bool Is { get; set; }

            #endregion
        }

        #endregion

        It should_be_is_left_selector_and_right_string = () => new ConditionalIs(() => Selector.Jquery.Id("id") == "True", true)
                                                                       .GetData()
                                                                       .ShouldEqualConditionalIs(left: "$('#id')",
                                                                                                 right: "True",
                                                                                                 method: "equal");

        It should_be_is_left_string_and_right_selector = () => new ConditionalIs(() => "True" == Selector.Jquery.Id("id"), true)
                                                                       .GetData()
                                                                       .ShouldEqualConditionalIs(left: "True",
                                                                                                 right: "$('#id')",
                                                                                                 method: "equal");

        It should_be_is_left_bool_and_right_selector = () => new ConditionalIs(() => true == Selector.Jquery.Id("id"), true)
                                                                     .GetData()
                                                                     .ShouldEqualConditionalIs(left: "True",
                                                                                               right: "$('#id')",
                                                                                               method: "equal");

        It should_be_is_left_selector_and_right_bool = () => new ConditionalIs(() => Selector.Jquery.Id("id") == true, true)
                                                                     .GetData()
                                                                     .ShouldEqualConditionalIs(left: "$('#id')",
                                                                                               right: "True",
                                                                                               method: "equal");

        It should_be_is_left_selector_and_right_cast = () => new ConditionalIs(() => Selector.Jquery.Id("id") == (int)ConditionalOfType.Eval, true)
                                                                     .GetData()
                                                                     .ShouldEqualConditionalIs(left: "$('#id')",
                                                                                               right: "2",
                                                                                               method: "equal");

        It should_be_is_left_cast_and_right_selector = () => new ConditionalIs(() => (int)ConditionalOfType.Eval == Selector.Jquery.Id("id"), true)
                                                                     .GetData()
                                                                     .ShouldEqualConditionalIs(left: "2",
                                                                                               right: "$('#id')",
                                                                                               method: "equal");

        It should_be_is_both_selector = () => new ConditionalIs(() => Selector.Jquery.Id("next") == Selector.Jquery.Id("id"), true)
                                                      .GetData()
                                                      .ShouldEqualConditionalIs(left: "$('#next')",
                                                                                right: "$('#id')",
                                                                                method: "equal");

        It should_be_is_unary_true = () => new ConditionalIs(() => true, true)
                                                   .GetData()
                                                   .ShouldEqualConditionalIs(left: "True",
                                                                             right: "True",
                                                                             method: "equal");

        It should_be_is_unary_false = () => new ConditionalIs(() => false, true)
                                                    .GetData()
                                                    .ShouldEqualConditionalIs(left: "True",
                                                                              right: "False",
                                                                              method: "equal");

        It should_be_is_unary_selector_true = () => new ConditionalIs(() => Selector.Jquery.Id("next"), true)
                                                            .GetData()
                                                            .ShouldEqualConditionalIs(left: "$('#next')",
                                                                                      right: "True",
                                                                                      method: "equal");

        It should_be_is_unary_selector_false = () => new ConditionalIs(() => !Selector.Jquery.Id("next"), true)
                                                             .GetData()
                                                             .ShouldEqualConditionalIs(left: "$('#next')",
                                                                                       right: "False",
                                                                                       method: "equal");

        It should_not_is_equal = () => new ConditionalIs(() => Selector.Jquery.Id("id") != true, true)
                                               .GetData()
                                               .ShouldEqualConditionalIs(left: "$('#id')",
                                                                         right: "True",
                                                                         method: "notequal");

        It should_be_is_constant = () => new ConditionalIs(() => true, true)
                                                 .GetData()
                                                 .ShouldEqualConditionalIs(left: "True",
                                                                           right: "True",
                                                                           method: "equal");

        It should_be_is_less_than = () => new ConditionalIs(() => Selector.Jquery.Id("id") < 6, true)
                                                  .GetData()
                                                  .ShouldEqualConditionalIs(left: "$('#id')",
                                                                            right: "6",
                                                                            method: "lessthan");

        It should_be_is_greater_than = () => new ConditionalIs(() => 5 > Selector.Jquery.Id("id"), true)
                                                     .GetData()
                                                     .ShouldEqualConditionalIs(left: "5",
                                                                               right: "$('#id')",
                                                                               method: "greaterthan");

        It should_be_is_less_than_or_equal = () => new ConditionalIs(() => Selector.Jquery.Id("id") <= Selector.Jquery.Id("id2"), true)
                                                           .GetData()
                                                           .ShouldEqualConditionalIs(left: "$('#id')",
                                                                                     right: "$('#id2')",
                                                                                     method: "lessthanorequal");

        It should_be_is_greater_than_or_equal = () => new ConditionalIs(() => Selector.Jquery.Id("id") >= Selector.Jquery.Id("id2"), true)
                                                              .GetData()
                                                              .ShouldEqualConditionalIs(left: "$('#id')",
                                                                                        right: "$('#id2')",
                                                                                        method: "greaterthanorequal");

        It should_be_selector_contains_value = () => new ConditionalIs(() => Selector.Jquery.Id("id").IsContains("aws"), true)
                                                             .GetData()
                                                             .ShouldEqualConditionalIs(left: "$('#id')",
                                                                                       right: "aws",
                                                                                       method: "iscontains");

        It should_be_value_contains_selector = () => new ConditionalIs(() => "aws".IsContains(Selector.Jquery.Id("id")), true)
                                                             .GetData()
                                                             .ShouldEqualConditionalIs(left: "aws",
                                                                                       right: "$('#id')",
                                                                                       method: "iscontains");

        It should_be_selector_contains_selector = () => new ConditionalIs(() => Selector.Jquery.Id("id").IsContains(Selector.Jquery.Self()), true)
                                                                .GetData()
                                                                .ShouldEqualConditionalIs(left: "$('#id')",
                                                                                          right: "$(this.self)",
                                                                                          method: "iscontains");

        It should_be_is_contains_equal_bool = () => new ConditionalIs(() => Selector.Jquery.Id("id").IsContains(Selector.Jquery.Self()) == false, true)
                                                            .GetData()
                                                            .ShouldEqualConditionalIs(left: "$('#id')",
                                                                                      right: "$(this.self)",
                                                                                      inverse: true,
                                                                                      method: "iscontains");

        It should_be_is_contains_false_native = () => new ConditionalIs(() => !Selector.Jquery.Id("id").IsContains(Selector.Jquery.Self()), true)
                                                              .GetData()
                                                              .ShouldEqualConditionalIs(left: "$('#id')",
                                                                                        right: "$(this.self)",
                                                                                        method: "iscontains",
                                                                                        inverse: true);

        It should_be_is_empty_on_selector = () => new ConditionalIs(() => Selector.Jquery.Id("id").IsEmpty(), true)
                                                          .GetData()
                                                          .ShouldEqualConditionalIs(left: "$('#id')",
                                                                                    right: string.Empty,
                                                                                    method: "isempty");

        It should_be_is_confirm = () => new ConditionalIs(() => Selector.JS.Confirm("Message"), true)
                                                .GetData()
                                                .ShouldEqualConditionalIs(left: "||javascript*confirm('Message')||",
                                                                          right: "True",
                                                                          method: "equal");

        It should_be_is_confirm_false = () => new ConditionalIs(() => !Selector.JS.Confirm("Message"), true)
                                                      .GetData()
                                                      .ShouldEqualConditionalIs(left: "||javascript*confirm('Message')||",
                                                                                right: "False",
                                                                                method: "equal");

        It should_be_is_field_true = () =>
                                         {
                                             var model = new FakeModel { Is = true };
                                             new ConditionalIs(() => model.Is, true)
                                                     .GetData()
                                                     .ShouldEqualConditionalIs(left: "True",
                                                                               right: "True",
                                                                               method: "equal");
                                         };  


        It should_be_is_field_false = () =>
                                         {
                                             var model = new FakeModel { Is = false };
                                             new ConditionalIs(() => model.Is, true)
                                                     .GetData()
                                                     .ShouldEqualConditionalIs(left: "True",
                                                                               right: "False",
                                                                               method: "equal");
                                         };      
        

        It should_be_is_equal_guid = () => new ConditionalIs(() => Selector.Jquery.Self() == Pleasure.Generator.TheSameGuid(), true)
                                                   .GetData()
                                                   .ShouldEqualConditionalIs(left: "$(this.self)",
                                                                             right: "dd2d6d88-d2e9-40e2-a60d-0e58ccd8235d",
                                                                             method: "equal");

        It should_be_is_equal_date_time = () => new ConditionalIs(() => Selector.Jquery.Self() == Pleasure.Generator.The20120406Noon(), true)
                                                        .GetData()
                                                        .ShouldEqualConditionalIs(left: "$(this.self)",
                                                                                  right: "4/16/2012 12:00:00 AM",
                                                                                  method: "equal");

        It should_be_method_for_value_true = () => new ConditionalIs(() => "id".IsAnyEqualsIgnoreCase("id", "abc"), true)
                                                           .GetData()
                                                           .ShouldEqualConditionalIs(left: "True",
                                                                                     right: "True",
                                                                                     method: "equal");

        It should_be_method_for_value_false = () => new ConditionalIs(() => "id".IsAnyEqualsIgnoreCase("aws", "wse"), true)
                                                            .GetData()
                                                            .ShouldEqualConditionalIs(left: "True",
                                                                                      right: "False",
                                                                                      method: "equal");
    }
}