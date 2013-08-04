namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ConditionalBuilder))]
    public class When_conditional_builder_reset_inverse
    {
        #region Estabilish value

        static string confirm1;

        static string confirm2;

        static ConditionalBuilder builder;

        #endregion

        Establish establish = () =>
                                  {
                                      confirm1 = Pleasure.Generator.String();
                                      confirm2 = Pleasure.Generator.String();

                                      builder = new ConditionalBuilder();
                                  };

        Because of = () => builder
                                   .Not
                                   .Confirm(Selector.Value(confirm1))
                                   .And
                                   .Confirm(Selector.Value(confirm2));

        It should_be_1_and = () => builder
                                           .GetByIndex(0)
                                           .GetData()
                                           .ShouldEqualWeak(new
                                                                {
                                                                        type = ConditionalOfType.Eval.ToString(), 
                                                                        inverse = true, 
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
    }
}