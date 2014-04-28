namespace Incoding.UnitTest.LinqSpecGroup
{
    #region << Using >>

    using System.Linq;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AdHocSpecification<>))]
    public class When_ad_hoc_specification
    {
        #region Establish value

        protected static IQueryable<string> testCollection;

        #endregion

        Establish establish = () => { testCollection = Pleasure.ToQueryable("Jose", "Manuel", "Julian"); };

        It should_be_negative = () => testCollection
                                              .Where((!new AdHocSpecification<string>(n => n.StartsWith("J"))).IsSatisfiedBy())
                                              .ToList()
                                              .Should(list =>
                                                          {
                                                              list.Count.ShouldEqual(1);
                                                              list[0].ShouldEqual("Manuel");
                                                          });

        It should_be_and_both_null = () => (new AdHocSpecification<string>(null) & new AdHocSpecification<string>(null))
                                                   .IsSatisfiedBy()
                                                   .ShouldBeNull();

        It should_be_and = () => testCollection
                                         .Where((new AdHocSpecification<string>(n => n.StartsWith("J")) & new AdHocSpecification<string>(n => n.EndsWith("e"))).IsSatisfiedBy())
                                         .ToList()
                                         .Should(list =>
                                                     {
                                                         list.Count.ShouldEqual(1);
                                                         list[0].ShouldEqual("Jose");
                                                     });

        It should_be_and_left_null = () => testCollection
                                                   .Where((new AdHocSpecification<string>(null) & new AdHocSpecification<string>(n => n.EndsWith("e"))).IsSatisfiedBy())
                                                   .ToList()
                                                   .Should(list =>
                                                               {
                                                                   list.Count.ShouldEqual(1);
                                                                   list[0].ShouldEqual("Jose");
                                                               });

        It should_be_and_right_null = () => testCollection
                                                    .Where((new AdHocSpecification<string>(n => n.EndsWith("e")) & new AdHocSpecification<string>(null)).IsSatisfiedBy())
                                                    .ToList()
                                                    .Should(list =>
                                                                {
                                                                    list.Count.ShouldEqual(1);
                                                                    list[0].ShouldEqual("Jose");
                                                                });

        It should_be_and_also = () => testCollection
                                              .Where((new AdHocSpecification<string>(n => n.StartsWith("J")) && new AdHocSpecification<string>(n => n.EndsWith("e"))).IsSatisfiedBy())
                                              .ToList()
                                              .Should(list =>
                                                          {
                                                              list.Count.ShouldEqual(1);
                                                              list[0].ShouldEqual("Jose");
                                                          });

        It should_be_or_both_null = () => (new AdHocSpecification<string>(null) || new AdHocSpecification<string>(null))
                                                  .IsSatisfiedBy()
                                                  .ShouldBeNull();

        It should_be_or = () => testCollection
                                        .Where((new AdHocSpecification<string>(n => n.StartsWith("J")) || new AdHocSpecification<string>(n => n.EndsWith("n"))).IsSatisfiedBy())
                                        .ToList()
                                        .Should(list =>
                                                    {
                                                        list.Count.ShouldEqual(2);
                                                        list[0].ShouldEqual("Jose");
                                                        list[1].ShouldEqual("Julian");
                                                    });

        It should_be_or_left_null = () => testCollection
                                                  .Where((new AdHocSpecification<string>(null) || new AdHocSpecification<string>(n => n.StartsWith("J"))).IsSatisfiedBy())
                                                  .ToList()
                                                  .Should(list =>
                                                              {
                                                                  list.Count.ShouldEqual(2);
                                                                  list[0].ShouldEqual("Jose");
                                                                  list[1].ShouldEqual("Julian");
                                                              });

        It should_be_or_right_null = () => testCollection
                                                   .Where((new AdHocSpecification<string>(n => n.StartsWith("J")) || new AdHocSpecification<string>(null)).IsSatisfiedBy())
                                                   .ToList()
                                                   .Should(list =>
                                                               {
                                                                   list.Count.ShouldEqual(2);
                                                                   list[0].ShouldEqual("Jose");
                                                                   list[1].ShouldEqual("Julian");
                                                               });
    }
}