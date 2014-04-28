namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ConditionalBuilder))]
    public class When_conditional_builder_binary
    {
        #region Establish value

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
                                   .Eval(confirm1)
                                   .And
                                   .Eval(confirm2)
                                   .Or
                                   .Eval(confirm3);

        It should_be_1_and = () => builder
                                           .GetByIndex(0)
                                           .GetData()
                                           .ShouldEqualWeak(new
                                                                {
                                                                        type = ConditionalOfType.Eval.ToString(), 
                                                                        inverse = false, 
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

        It should_be_3_or = () => builder
                                          .GetByIndex(2)
                                          .GetData()
                                          .ShouldEqualWeak(new
                                                               {
                                                                       type = ConditionalOfType.Eval.ToString(), 
                                                                       inverse = false, 
                                                                       and = false,
                                                                       code = confirm3
                                                               }, dsl => dsl.IncludeAllFields());
    }
}