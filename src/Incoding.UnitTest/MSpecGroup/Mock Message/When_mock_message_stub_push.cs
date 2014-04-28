namespace Incoding.UnitTest.MSpecGroup
{
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_stub_push
    {
        #region Fake classes

        class FakeMockMessage : CommandBase
        {
            #region Override

            public string Id { get; set; }

            #endregion

            public override void Execute()
            {
                this.Dispatcher.Push(new FakeMockMessage
                                    {
                                            Id = Pleasure.Generator.TheSameString()
                                    });
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
                                              .StubPush(new FakeMockMessage
                                                            {
                                                                    Id = Pleasure.Generator.TheSameString()
                                                            });
                                  };

        Because of = () => mockMessage.Original.Execute();

        It should_be_empty_result = () => mockMessage.ShouldBePushed();
    }
}