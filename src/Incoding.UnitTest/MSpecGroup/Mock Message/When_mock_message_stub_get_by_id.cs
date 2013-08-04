namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_stub_get_by_id
    {
        #region Fake classes

        class FakeMockMessage : QueryBase<IEntity>
        {
            #region Override

            #region Properties

            public string Id { get; set; }

            #endregion

            protected override IEntity ExecuteResult()
            {
                return Repository.GetById<IEntity>(Id);
            }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MockMessage<FakeMockMessage, IEntity> mockMessage;

        #endregion

        Establish establish = () =>
                                  {
                                      var input = Pleasure.Generator.Invent<FakeMockMessage>();
                                      mockMessage = MockQuery<FakeMockMessage, IEntity>
                                              .When(input)
                                              .StubGetById(input.Id, Pleasure.MockAsObject<IEntity>());
                                  };

        Because of = () => mockMessage.Original.Execute();

        It should_be_empty_result = () => mockMessage.ShouldBeIsResult(entity => entity.ShouldNotBeNull());
    }
}