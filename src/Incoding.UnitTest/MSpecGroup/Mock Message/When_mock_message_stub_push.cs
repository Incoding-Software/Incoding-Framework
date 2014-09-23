namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

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
                Dispatcher.Push(new FakeMockMessage { Id = Pleasure.Generator.TheSameString() });
            }
        }

        class FakeMockMessageWithDoublePushSameCommand : CommandBase
        {

            public override void Execute()
            {
                Dispatcher.Push(new FakeMockMessage { Id = "1" });
                Dispatcher.Push(new FakeMockMessage { Id = "2" });
            }
        }

        #endregion

        #region Establish value

        static MockMessage<FakeMockMessage, object> mockMessage;

        #endregion

        It should_be_stub_push = () =>
                                 {
                                     var input = Pleasure.Generator.Invent<FakeMockMessage>();
                                     var mockMessage = MockCommand<FakeMockMessage>
                                             .When(input)
                                             .StubPush(new FakeMockMessage
                                                       {
                                                               Id = Pleasure.Generator.TheSameString()
                                                       });
                                     mockMessage.Original.Execute();
                                     mockMessage.ShouldPushed();
                                 };

        It should_be_stub_push_invent = () =>
                                        {
                                            var input = Pleasure.Generator.Invent<FakeMockMessage>();
                                            var mockMessage = MockCommand<FakeMockMessage>
                                                    .When(input)
                                                    .StubPush<FakeMockMessage>(dsl => dsl.Tuning(r => r.Id, Pleasure.Generator.TheSameString()));
                                            mockMessage.Original.Execute();
                                            mockMessage.ShouldPushed();
                                        };

        It should_be_stub_push_double_same_command = () =>
                                                     {
                                                         var input = Pleasure.Generator.Invent<FakeMockMessageWithDoublePushSameCommand>();
                                                         var mockMessage = MockCommand<FakeMockMessageWithDoublePushSameCommand>
                                                                 .When(input)
                                                                 .StubPush<FakeMockMessage>(dsl => dsl.Tuning(r => r.Id, "2"))
                                                                 .StubPush<FakeMockMessage>(dsl => dsl.Tuning(r => r.Id, "1"));
                                                         mockMessage.Original.Execute();
                                                         mockMessage.ShouldPushed();
                                                     };
    }
}