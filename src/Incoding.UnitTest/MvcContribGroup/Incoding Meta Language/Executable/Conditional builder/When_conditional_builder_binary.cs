namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(ConditionalBuilder))]
    public class When_conditional_builder_binary
    {
        #region Estabilish value

        static string confirm1;

        static string confirm2;

        static ConditionalBuilder builder;

        static string confirm3;

        #endregion

        Establish establish = () =>
                                  {
                                      confirm1 = Pleasure.Generator.String();
                                      confirm2 = Pleasure.Generator.String();
                                      confirm3 = Pleasure.Generator.String();
                                      builder = new ConditionalBuilder();
                                  };

        Because of = () => builder
                                   .Confirm(Selector.Value(confirm1))
                                   .And
                                   .Confirm(Selector.Value(confirm2))
                                   .Or
                                   .Confirm(Selector.Value(confirm3));

        It should_be_1_and = () => builder
                                           .GetByIndex(0)
                                           .GetData()
                                           .ShouldEqualWeak(new
                                                                {
                                                                        type = ConditionalOfType.Eval.ToString(), 
                                                                        inverse = false, 
                                                                        and = true, 
                                                                        code = "window.confirm(this.tryGetVal('{0}'));".F(confirm1)
                                                                }, dsl => dsl.IncludeAllFields());

        It should_be_2_and = () => builder
                                           .GetByIndex(1)
                                           .GetData()
                                           .ShouldEqualWeak(new
                                                                {
                                                                        type = ConditionalOfType.Eval.ToString(), 
                                                                        inverse = false, 
                                                                        and = true, 
                                                                        code = "window.confirm(this.tryGetVal('{0}'));".F(confirm2)
                                                                }, dsl => dsl.IncludeAllFields());

        It should_be_3_or = () => builder
                                          .GetByIndex(2)
                                          .GetData()
                                          .ShouldEqualWeak(new
                                                               {
                                                                       type = ConditionalOfType.Eval.ToString(), 
                                                                       inverse = false, 
                                                                       and = false, 
                                                                       code = "window.confirm(this.tryGetVal('{0}'));".F(confirm3)
                                                               }, dsl => dsl.IncludeAllFields());
    }
}