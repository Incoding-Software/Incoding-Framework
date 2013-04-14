namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(GetEntityByIdQuery<>))]
    public class When_get_entity_by_id
    {
        #region Estbilish value

        #region Fake classes

        class FakeEntity : IncEntityBase { }

        #endregion

        #region Estabilish value

        static MockMessage<GetEntityByIdQuery<FakeEntity>, FakeEntity> mockQuery;

        #endregion

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<GetEntityByIdQuery<FakeEntity>>();
                                      mockQuery = MockQuery<GetEntityByIdQuery<FakeEntity>, FakeEntity>
                                              .When(query)
                                              .StubGetById(query.Id, Pleasure.Generator.Invent<FakeEntity>());
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(entity => entity.ShouldNotBeNull());
    }
}