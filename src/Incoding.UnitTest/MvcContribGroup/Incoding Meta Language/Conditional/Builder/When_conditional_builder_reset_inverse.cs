namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ConditionalBuilder))]
    public class When_conditional_builder_reset_inverse
    {
        #region Establish value

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
                                   .Eval(confirm1)
                                   .And
                                   .Eval(confirm2);

        It should_be_1_and = () => builder
                                           .GetByIndex(0)
                                           .GetData()
                                           .ShouldEqualWeak(new
                                                                {
                                                                        type = ConditionalOfType.Eval.ToString(),
                                                                        inverse = true,
                                                                        and = true,
                                                                        code = confirm1
                                                                }, dsl => dsl.IncludeAllFields());

        It should_be_2_and = () => builder
                                           .GetByIndex(1)
                                           .GetData()
                                           .ShouldEqualWeak(new
                                                                {
                                                                        type = ConditionalOfType.Eval.ToString(),
                                                                        inverse = false,
                                                                        and = true,
                                                                        code = confirm2
                                                                }, dsl => dsl.IncludeAllFields());
    }
}