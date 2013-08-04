namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetEntitiesQuery<>))]
    public class When_get_entities
    {
        #region Fake classes

        class FakeEntity : IncEntityBase { }

        #endregion

        #region Estabilish value

        static MockMessage<GetEntitiesQuery<FakeEntity>, List<FakeEntity>> mockQuery;

        #endregion

        Establish establish = () =>
                                  {
                                      mockQuery = MockQuery<GetEntitiesQuery<FakeEntity>, List<FakeEntity>>
                                              .When(new GetEntitiesQuery<FakeEntity>())
                                              .StubNotEmptyQuery<FakeEntity>();
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(list => list.ShouldNotBeEmpty());
    }
}