namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ConditionalBase))]
    public class When_conditional_base
    {
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

        It should_be_eval = () => new ConditionalEval("code", true)
                                          .GetData()
                                          .ShouldEqualWeak(new
                                                               {
                                                                       type = ConditionalOfType.Eval.ToString(), 
                                                                       inverse = false, 
                                                                       code = "code", 
                                                                       and = true
                                                               }, dsl => dsl.IncludeAllFields());
    }
}