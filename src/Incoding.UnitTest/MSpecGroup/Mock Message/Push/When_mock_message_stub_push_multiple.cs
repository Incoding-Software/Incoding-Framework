namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

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

            protected override void Execute()
            {
                Dispatcher.Push(new FakeMockMessage { Id = "Id1" });
                Dispatcher.Push(new FakeMockMessage { Id = "Id2" });
            }
        }

        #endregion

        #region Establish value

        static MockMessage<FakeMockMessage, object> mockMessage;

        static Exception exception;

        #endregion

        Establish establish = () =>
                              {
                                  var input = Pleasure.Generator.Invent<FakeMockMessage>();
                                  mockMessage = MockCommand<FakeMockMessage>
                                          .When(input)
                                          .StubPush(new FakeMockMessage { Id = "Id1" })
                                          .StubPush(new FakeMockMessage { Id = "Id2" });
                              };

        Because of = () => { exception = Catch.Exception(() => mockMessage.Execute()); };

        It should_be_success = () => exception.ShouldBeNull();
    }
}