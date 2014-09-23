namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_stub_get_by_id
    {
        #region Fake classes

        class FakeMockMessage : QueryBase<FakeEntityForNew>
        {
            #region Override

            #region Properties

            public string Id { get; set; }

            #endregion

            protected override FakeEntityForNew ExecuteResult()
            {
                return Repository.GetById<FakeEntityForNew>(Id);
            }

            #endregion
        }

        #endregion

        #region Establish value

        static MockMessage<FakeMockMessage, FakeEntityForNew> mockMessage;

        #endregion

        Establish establish = () =>
                              {
                                  var input = Pleasure.Generator.Invent<FakeMockMessage>();
                                  mockMessage = MockQuery<FakeMockMessage, FakeEntityForNew>
                                          .When(input)
                                          .StubGetById(input.Id, Pleasure.MockAsObject<FakeEntityForNew>());
                              };

        Because of = () => mockMessage.Original.Execute();

        It should_be_empty_result = () => mockMessage.ShouldBeIsResult(entity => entity.ShouldNotBeNull());
    }
}