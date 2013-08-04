namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

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
                                                                                code = "ExecutableHelper.Compare(this.tryGetVal($('#id')), this.tryGetVal('2'), 'equal');", 
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

        It should_be_is_field_bool = () =>
                                         {
                                             var model = new FakeModel { Is = true };
                                             new ConditionalIs(() => model.Is, true)
                                                     .GetData()
                                                     .ShouldEqualWeak(new
                                                                          {
                                                                                  type = ConditionalOfType.Eval.ToString(), 
                                                                                  inverse = false, 
                                                                                  and = true, 
                                                                                  code = "ExecutableHelper.Compare(this.tryGetVal('True'), this.tryGetVal('True'), 'equal');"
                                                                          }, dsl => dsl.IncludeAllFields());
                                         };
    }
}