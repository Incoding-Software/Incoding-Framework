namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(HasEntitiesQuery<>))]
    public class When_has_entities
    {
        #region Establish value

        static MockMessage<HasEntitiesQuery<FakeEntityForNew>, IncBoolResponse> mockQuery;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<HasEntitiesQuery<FakeEntityForNew>>();

                                      mockQuery = MockQuery<HasEntitiesQuery<FakeEntityForNew>, IncBoolResponse>
                                              .When(query)
                                              .StubQuery(entities: Pleasure.MockAsObject<FakeEntityForNew>());
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(true);
    }
}