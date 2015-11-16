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
        class FakeModel
        {
            public bool IsCTS { get; set; }

            public bool IsWBD { get; set; }

            public bool IsFeature { get; set; }
        }

        #region Fundamental

        It should_be_not = () => new ConditionalBuilder()
                                         .Not
                                         .Eval(Selector.Value(Pleasure.Generator.TheSameString()))
                                         .GetByIndex(0)
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
                                          .GetByIndex(0)
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
                                        .GetByIndex(0)
                                        .GetData()
                                        .ShouldEqualConditionalIs(left: "$('#id')",
                                                                  right: "||value*True||",
                                                                  method: "equal");

        It should_be_is_inc_455_evaluated = () =>
                                            {
                                                var model = new FakeModel()
                                                            {
                                                                    IsCTS = true,
                                                                    IsWBD = false,
                                                                    IsFeature = true
                                                            };

                                                var builder = new ConditionalBuilder().Is(() => model.IsFeature && (model.IsCTS && model.IsWBD));
                                                builder
                                                        .GetByIndex(0)
                                                        .GetData()
                                                        .ShouldEqualConditionalIs(left: "||value*True||",
                                                                                  right: "||value*True||",
                                                                                  method: "equal");

                                                builder
                                                        .GetByIndex(1)
                                                        .GetData()
                                                        .ShouldEqualConditionalIs(left: "||value*True||",
                                                                                  right: "||value*True||",
                                                                                  method: "equal");

                                                builder
                                                        .GetByIndex(2)
                                                        .GetData()
                                                        .ShouldEqualConditionalIs(left: "||value*True||",
                                                                                  right: "||value*False||",
                                                                                  method: "equal");
                                            };

        It should_be_is_and = () => new ConditionalBuilder()
                                            .Is(() => Selector.Jquery.Id("id") &&
                                                      !Selector.Jquery.Id("id2"))
                                            .Should(builder =>
                                                    {
                                                        builder.GetByIndex(0)
                                                               .GetData()
                                                               .ShouldEqualConditionalIs(left: "$('#id')",
                                                                                         right: "||value*True||",
                                                                                         method: "equal");

                                                        builder.GetByIndex(1)
                                                               .GetData()
                                                               .ShouldEqualConditionalIs(left: "$('#id2')",
                                                                                         right: "||value*False||",
                                                                                         method: "equal");
                                                    });

        It should_be_is_or = () => new ConditionalBuilder()
                                           .Is(() => Selector.Jquery.Id("id") ||
                                                     !Selector.Jquery.Id("id2"))
                                           .Should(builder =>
                                                   {
                                                       builder.GetByIndex(0)
                                                              .GetData()
                                                              .ShouldEqualConditionalIs(left: "$('#id')",
                                                                                        right: "||value*True||",
                                                                                        and: false,
                                                                                        method: "equal");

                                                       builder.GetByIndex(1)
                                                              .GetData()
                                                              .ShouldEqualConditionalIs(left: "$('#id2')",
                                                                                        right: "||value*False||",
                                                                                        and: false,
                                                                                        method: "equal");
                                                   });

        It should_be_is_multiple = () => new ConditionalBuilder().Is(() => Selector.Jquery.Id("id") || !Selector.Jquery.Id("id2") && Selector.Jquery.Id("id3"))
                                                                 .Should(builder =>
                                                                         {
                                                                             builder.GetByIndex(0)
                                                                                    .GetData()
                                                                                    .ShouldEqualConditionalIs(left: "$('#id')",
                                                                                                              right: "||value*True||",
                                                                                                              and: false,
                                                                                                              method: "equal");

                                                                             builder.GetByIndex(1)
                                                                                    .GetData()
                                                                                    .ShouldEqualConditionalIs(left: "$('#id2')",
                                                                                                              right: "||value*False||",
                                                                                                              and: true,
                                                                                                              method: "equal");

                                                                             builder.GetByIndex(2)
                                                                                    .GetData()
                                                                                    .ShouldEqualConditionalIs(left: "$('#id3')",
                                                                                                              right: "||value*True||",
                                                                                                              and: true,
                                                                                                              method: "equal");
                                                                         });

        It should_be_data = () => new ConditionalBuilder()
                                          .Data<IEntity>(r => r.Id == "123")
                                          .GetByIndex(0)
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