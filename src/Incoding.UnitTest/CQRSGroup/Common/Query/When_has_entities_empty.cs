namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(HasEntitiesQuery<IEntity>))]
    public class When_has_entities_empty
    {
        #region Estabilish value

        static MockMessage<HasEntitiesQuery<IEntity>, IncStructureResponse<bool>> mockQuery;

        static IncStructureResponse<bool> expected;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<HasEntitiesQuery<IEntity>>();
                                      expected = new IncStructureResponse<bool>(false);

                                      mockQuery = MockQuery<HasEntitiesQuery<IEntity>, IncStructureResponse<bool>>
                                              .When(query);
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);
    }
}