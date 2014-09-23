namespace Incoding.UnitTest.MSpecGroup
{
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_stub_push_multiple
    {
        #region Fake classes

        class FakeMockMessage : CommandBase
        {
            #region Override

            public string Id { get; set; }
            public string Id2 { get; set; }

            #endregion

            public override void Execute()
            {
                this.Dispatcher.Push(new FakeMockMessage { Id = "Id1" });
                this.Dispatcher.Push(new FakeMockMessage { Id = "Id2"});
            }
        }

        #endregion

        #region Establish value

        static MockMessage<FakeMockMessage, object> mockMessage;

        #endregion

        Establish establish = () =>
                                  {
                                      var input = Pleasure.Generator.Invent<FakeMockMessage>();
                                      mockMessage = MockCommand<FakeMockMessage>
                                              .When(input)
                                              .StubPush(new FakeMockMessage { Id = "Id1" })
                                              .StubPush(new FakeMockMessage { Id = "Id2" });
                                  };

        Because of = () => mockMessage.Original.Execute();

        It should_be_pushed = () => mockMessage.ShouldPushed();
    }
}