namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ConditionalBuilder))]
    public class When_conditional_builder
    {
        #region Fundamental

        It should_be_not = () => new ConditionalBuilder()
                                         .Not
                                         .Eval(Selector.Value(Pleasure.Generator.TheSameString()))
                                         .GetFirst()
                                         .GetData()
                                         .ShouldEqualWeak(new
                                                              {
                                                                      type = ConditionalOfType.Eval.ToString(), 
                                                                      inverse = true, 
                                                                      and = true, 
                                                                      code = "TheSameString"
                                                              }, dsl => dsl.IncludeAllFields());

        #endregion

        #region Imp

        It should_be_eval = () => new ConditionalBuilder()
                                          .Eval(Pleasure.Generator.TheSameString())
                                          .GetFirst()
                                          .GetData()
                                          .ShouldEqualWeak(new
                                                               {
                                                                       type = ConditionalOfType.Eval.ToString(), 
                                                                       inverse = false, 
                                                                       and = true, 
                                                                       code = "TheSameString"
                                                               }, dsl => dsl.IncludeAllFields());

        It should_be_is = () => new ConditionalBuilder()
                                        .Is(() => Selector.Jquery.Id("id") == true)
                                        .GetFirst()
                                        .GetData()
                                        .ShouldEqualConditionalIs(left: "$('#id')",
                                                                  right: "True", 
                                                                  method: "equal");

        It should_be_data = () => new ConditionalBuilder()
                                          .Data<IEntity>(r => r.Id == "123")
                                          .GetFirst()
                                          .GetData()
                                          .ShouldEqualWeak(new
                                                               {
                                                                       type = ConditionalOfType.Data.ToString(), 
                                                                       inverse = false, 
                                                                       property = "Id", 
                                                                       value = "123", 
                                                                       method = "equal", 
                                                                       and = true
                                                               }, dsl => dsl.IncludeAllFields());

        #endregion
    }
}