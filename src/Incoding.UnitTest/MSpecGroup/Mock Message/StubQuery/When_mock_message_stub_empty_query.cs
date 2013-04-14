namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_stub_empty_query
    {
        #region Fake classes

        class FakeMockMessage : QueryBase<List<IEntity>>
        {
            #region Override

            protected override List<IEntity> ExecuteResult()
            {
                return Repository.Query<IEntity>().ToList();
            }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MockMessage<FakeMockMessage, List<IEntity>> mockMessage;

        #endregion

        Establish establish = () =>
                                  {
                                      mockMessage = MockQuery<FakeMockMessage, List<IEntity>>
                                              .When(new FakeMockMessage())
                                              .StubEmptyQuery<IEntity>();
                                  };

        Because of = () => mockMessage.Original.Execute();

        It should_be_empty_result = () => mockMessage.ShouldBeIsResult(list => list.ShouldBeEmpty());
    }
}