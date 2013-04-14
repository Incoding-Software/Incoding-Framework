namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(ConditionalBase))]
    public class When_conditional_get_data
    {
        #region Fake classes

        class FakeModel
        {
            #region Properties

            public string Prop1 { get; set; }

            public bool PropBool { get; set; }

            #endregion
        }

        #endregion

        It should_be_true_with_inverse = () =>
                                             {
                                                 var incodingConditionalEmpty = !new ConditionalEval("code", true);
                                                 incodingConditionalEmpty
                                                         .GetData()
                                                         .ShouldEqualWeak(new
                                                                              {
                                                                                      type = ConditionalOfType.Eval.ToString(), 
                                                                                      code = "code", 
                                                                                      inverse = true, 
                                                                                      and = true
                                                                              }, dsl => dsl.IncludeAllFields());
                                             };

        It should_be_url = () =>
                               {
                                   IDictionary<string, object> optionCollections = new Dictionary<string, object>();
                                   optionCollections["url"] = Pleasure.Generator.TheSameString();
                                   optionCollections["async"] = false;
                                   optionCollections["type"] = "DELETE";

                                   new ConditionalUrl(Pleasure.Generator.TheSameString(), options => options.Type = HttpVerbs.Delete, true)
                                           .GetData()
                                           .ShouldEqualWeak(new
                                                                {
                                                                        type = ConditionalOfType.Url.ToString(), 
                                                                        inverse = false, 
                                                                        ajax = optionCollections, 
                                                                        and = true
                                                                }, dsl => dsl.IncludeAllFields());
                               };

        It should_be_eval = () => new ConditionalEval("code", true)
                                          .GetData()
                                          .ShouldEqualWeak(new
                                                               {
                                                                       type = ConditionalOfType.Eval.ToString(), 
                                                                       inverse = false, 
                                                                       code = "code", 
                                                                       and = true
                                                               }, dsl => dsl.IncludeAllFields());

        #region Data

        It should_be_data_equal_constant = () => new ConditionalData<FakeModel>(r => r.Prop1 == "123", true)
                                                         .GetData()
                                                         .ShouldEqualWeak(new
                                                                              {
                                                                                      type = ConditionalOfType.Data.ToString(), 
                                                                                      inverse = false, 
                                                                                      property = "Prop1", 
                                                                                      value = "123", 
                                                                                      method = "equal", 
                                                                                      and = true
                                                                              }, dsl => dsl.IncludeAllFields());

        It should_be_data_equal_bool = () => new ConditionalData<FakeModel>(r => r.PropBool, true)
                                                     .GetData()
                                                     .ShouldEqualWeak(new
                                                                          {
                                                                                  type = ConditionalOfType.Data.ToString(), 
                                                                                  inverse = false, 
                                                                                  property = "PropBool", 
                                                                                  value = "True", 
                                                                                  method = "equal", 
                                                                                  and = true
                                                                          }, dsl => dsl.IncludeAllFields());

        It should_be_data_equal_to_context = () => new ConditionalData<FakeModel>(r => r.Prop1 == Selector.Jquery.Self(), true)
                                                           .GetData()
                                                           .ShouldEqualWeak(new
                                                                                {
                                                                                        type = ConditionalOfType.Data.ToString(), 
                                                                                        inverse = false, 
                                                                                        property = "Prop1", 
                                                                                        value = "$(this.self)", 
                                                                                        method = "equal", 
                                                                                        and = true
                                                                                }, dsl => dsl.IncludeAllFields());

        It should_be_data_not_equal = () => new ConditionalData<FakeModel>(r => r.Prop1 != "123", true)
                                                    .GetData()
                                                    .ShouldEqualWeak(new
                                                                         {
                                                                                 type = ConditionalOfType.Data.ToString(), 
                                                                                 inverse = false, 
                                                                                 property = "Prop1", 
                                                                                 value = "123", 
                                                                                 method = "notequal", 
                                                                                 and = true
                                                                         }, dsl => dsl.IncludeAllFields());

        It should_be_data_equal_with_method = () => new ConditionalData<FakeModel>(r => r.Prop1 == 123.ToString(), true)
                                                            .GetData()
                                                            .ShouldEqualWeak(new
                                                                                 {
                                                                                         type = ConditionalOfType.Data.ToString(), 
                                                                                         inverse = false, 
                                                                                         property = "Prop1", 
                                                                                         value = "123", 
                                                                                         method = "equal", 
                                                                                         and = true
                                                                                 }, dsl => dsl.IncludeAllFields());

        It should_be_data_contains = () => new ConditionalData<FakeModel>(r => r.Prop1.Contains("123"), true)
                                                   .GetData()
                                                   .ShouldEqualWeak(new
                                                                        {
                                                                                type = ConditionalOfType.Data.ToString(), 
                                                                                inverse = false, 
                                                                                property = "Prop1", 
                                                                                value = "123", 
                                                                                method = "contains", 
                                                                                and = true
                                                                        }, dsl => dsl.IncludeAllFields());

        It should_be_data_is_empty = () => new ConditionalData<FakeModel>(r => r.IsEmpty(), true)
                                                   .GetData()
                                                   .ShouldEqualWeak(new
                                                                        {
                                                                                type = ConditionalOfType.Data.ToString(), 
                                                                                inverse = false, 
                                                                                property = string.Empty, 
                                                                                value = string.Empty, 
                                                                                method = "isempty", 
                                                                                and = true
                                                                        }, dsl => dsl.IncludeAllFields());

        It should_be_data_is_empty_with_property = () => new ConditionalData<FakeModel>(r => r.Prop1.IsEmpty(), true)
                                                                 .GetData()
                                                                 .ShouldEqualWeak(new
                                                                                      {
                                                                                              type = ConditionalOfType.Data.ToString(), 
                                                                                              inverse = false, 
                                                                                              property = "Prop1", 
                                                                                              value = string.Empty, 
                                                                                              method = "isempty", 
                                                                                              and = true
                                                                                      }, dsl => dsl.IncludeAllFields());

        It should_be_data_is_id = () => new ConditionalDataIsId<FakeModel>(r => r.Prop1, true)
                                                .GetData()
                                                .ShouldEqualWeak(new
                                                                     {
                                                                             type = ConditionalOfType.DataIsId.ToString(), 
                                                                             inverse = false, 
                                                                             property = "Prop1", 
                                                                             and = true
                                                                     }, dsl => dsl.IncludeAllFields());

        #endregion

        #region Is

        It should_be_is_right_not_selector = () => new ConditionalIs(() => Selector.Jquery.Id("id") == true, true)
                                                           .GetData()
                                                           .ShouldEqualWeak(new
                                                                                {
                                                                                        type = ConditionalOfType.Eval.ToString(), 
                                                                                        inverse = false, 
                                                                                        code = "ExecutableHelper.Compare(this.tryGetVal($('#id')), this.tryGetVal('True'), 'equal');", 
                                                                                        and = true
                                                                                }, dsl => dsl.IncludeAllFields());

        It should_be_is_left_convert = () => new ConditionalIs(() => Selector.Jquery.Id("id") == "True", true)
                                                     .GetData()
                                                     .ShouldEqualWeak(new
                                                                          {
                                                                                  type = ConditionalOfType.Eval.ToString(), 
                                                                                  inverse = false, 
                                                                                  code = "ExecutableHelper.Compare(this.tryGetVal($('#id')), this.tryGetVal('True'), 'equal');", 
                                                                                  and = true
                                                                          }, dsl => dsl.IncludeAllFields());

        It should_be_is_right_convert = () => new ConditionalIs(() => "True" == Selector.Jquery.Id("id"), true)
                                                      .GetData()
                                                      .ShouldEqualWeak(new
                                                                           {
                                                                                   type = ConditionalOfType.Eval.ToString(), 
                                                                                   inverse = false, 
                                                                                   code = "ExecutableHelper.Compare(this.tryGetVal('True'), this.tryGetVal($('#id')), 'equal');", 
                                                                                   and = true
                                                                           }, dsl => dsl.IncludeAllFields());

        It should_be_is_right_cast = () => new ConditionalIs(() => Selector.Jquery.Id("id") == (int)ConditionalOfType.Eval, true)
                                                   .GetData()
                                                   .ShouldEqualWeak(new
                                                                        {
                                                                                type = ConditionalOfType.Eval.ToString(), 
                                                                                inverse = false, 
                                                                                code = "ExecutableHelper.Compare(this.tryGetVal($('#id')), this.tryGetVal('3'), 'equal');", 
                                                                                and = true
                                                                        }, dsl => dsl.IncludeAllFields());

        It should_be_is_left_not_selector = () => new ConditionalIs(() => true == Selector.Jquery.Id("id"), true)
                                                          .GetData()
                                                          .ShouldEqualWeak(new
                                                                               {
                                                                                       type = ConditionalOfType.Eval.ToString(), 
                                                                                       inverse = false, 
                                                                                       code = "ExecutableHelper.Compare(this.tryGetVal('True'), this.tryGetVal($('#id')), 'equal');", 
                                                                                       and = true
                                                                               }, dsl => dsl.IncludeAllFields());

        It should_be_is_both_selector = () => new ConditionalIs(() => Selector.Jquery.Id("next") == Selector.Jquery.Id("id"), true)
                                                      .GetData()
                                                      .ShouldEqualWeak(new
                                                                           {
                                                                                   type = ConditionalOfType.Eval.ToString(), 
                                                                                   inverse = false, 
                                                                                   code = "ExecutableHelper.Compare(this.tryGetVal($('#next')), this.tryGetVal($('#id')), 'equal');", 
                                                                                   and = true
                                                                           }, dsl => dsl.IncludeAllFields());

        It should_not_is_be_equal = () => new ConditionalIs(() => Selector.Jquery.Id("id") != Selector.Value(true), true)
                                                  .GetData()
                                                  .ShouldEqualWeak(new
                                                                       {
                                                                               type = ConditionalOfType.Eval.ToString(), 
                                                                               inverse = false, 
                                                                               code = "ExecutableHelper.Compare(this.tryGetVal($('#id')), this.tryGetVal('True'), 'notequal');", 
                                                                               and = true
                                                                       }, dsl => dsl.IncludeAllFields());

        It should_be_is_constant = () => new ConditionalIs(() => true, true)
                                                 .GetData()
                                                 .ShouldEqualWeak(new
                                                                      {
                                                                              type = ConditionalOfType.Eval.ToString(), 
                                                                              inverse = false, 
                                                                              code = "ExecutableHelper.Compare(this.tryGetVal('True'), this.tryGetVal('True'), 'equal');", 
                                                                              and = true
                                                                      }, dsl => dsl.IncludeAllFields());

        It should_be_is_less_than = () => new ConditionalIs(() => Selector.Value(5) < Selector.Value(6), true)
                                                  .GetData()
                                                  .ShouldEqualWeak(new
                                                                       {
                                                                               type = ConditionalOfType.Eval.ToString(), 
                                                                               inverse = false, 
                                                                               code = "ExecutableHelper.Compare(this.tryGetVal('5'), this.tryGetVal('6'), 'lessthan');", 
                                                                               and = true
                                                                       }, dsl => dsl.IncludeAllFields());

        It should_be_is_greater_than = () => new ConditionalIs(() => Selector.Value(5) > Selector.Value(6), true)
                                                     .GetData()
                                                     .ShouldEqualWeak(new
                                                                          {
                                                                                  type = ConditionalOfType.Eval.ToString(), 
                                                                                  inverse = false, 
                                                                                  code = "ExecutableHelper.Compare(this.tryGetVal('5'), this.tryGetVal('6'), 'greaterthan');", 
                                                                                  and = true
                                                                          }, dsl => dsl.IncludeAllFields());

        It should_be_is_less_than_or_equal = () => new ConditionalIs(() => Selector.Value(5) <= Selector.Value(6), true)
                                                           .GetData()
                                                           .ShouldEqualWeak(new
                                                                                {
                                                                                        type = ConditionalOfType.Eval.ToString(), 
                                                                                        inverse = false, 
                                                                                        code = "ExecutableHelper.Compare(this.tryGetVal('5'), this.tryGetVal('6'), 'lessthanorequal');", 
                                                                                        and = true
                                                                                }, dsl => dsl.IncludeAllFields());

        It should_be_is_greater_than_or_equal = () => new ConditionalIs(() => Selector.Value(5) >= Selector.Value(6), true)
                                                              .GetData()
                                                              .ShouldEqualWeak(new
                                                                                   {
                                                                                           type = ConditionalOfType.Eval.ToString(), 
                                                                                           inverse = false, 
                                                                                           code = "ExecutableHelper.Compare(this.tryGetVal('5'), this.tryGetVal('6'), 'greaterthanorequal');", 
                                                                                           and = true
                                                                                   }, dsl => dsl.IncludeAllFields());

        It should_be_is_contains_argument_not_selector = () => new ConditionalIs(() => Selector.Jquery.Id("id").Contains("aws"), true)
                                                                       .GetData()
                                                                       .ShouldEqualWeak(new
                                                                                            {
                                                                                                    type = ConditionalOfType.Eval.ToString(), 
                                                                                                    inverse = false, 
                                                                                                    and = true, 
                                                                                                    code = "ExecutableHelper.Compare(this.tryGetVal($('#id')), this.tryGetVal('aws'), 'contains');"
                                                                                            }, dsl => dsl.IncludeAllFields());

        It should_be_is_contains_selector = () => new ConditionalIs(() => Selector.Jquery.Id("id").Contains(Selector.Jquery.Self()), true)
                                                          .GetData()
                                                          .ShouldEqualWeak(new
                                                                               {
                                                                                       type = ConditionalOfType.Eval.ToString(), 
                                                                                       inverse = false, 
                                                                                       and = true, 
                                                                                       code = "ExecutableHelper.Compare(this.tryGetVal($('#id')), this.tryGetVal($(this.self)), 'contains');"
                                                                               }, dsl => dsl.IncludeAllFields());

        It should_be_is_empty_on_selector = () => new ConditionalIs(() => Selector.Jquery.Id("id").IsEmpty(), true)
                                                          .GetData()
                                                          .ShouldEqualWeak(new
                                                                               {
                                                                                       type = ConditionalOfType.Eval.ToString(), 
                                                                                       inverse = false, 
                                                                                       and = true, 
                                                                                       code = "ExecutableHelper.Compare(this.tryGetVal($('#id')), this.tryGetVal(), 'isempty');"
                                                                               }, dsl => dsl.IncludeAllFields());

        #endregion
    }
}