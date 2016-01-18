namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_stub_push_multiple_unique
    {
        Establish establish = () =>
                              {
                                  var input = Pleasure.Generator.Invent<FakeMockMessage>();
                                  mockMessage = MockCommand<FakeMockMessage>
                                          .When(input)
                                          .StubPush(new FakeCommand { Name = "Name" })
                                          .StubPush(new FakeCommand2 { Name2 = "Name2" });
                              };

        Because of = () => { exception = Catch.Exception(() => mockMessage.Execute()); };

        It should_be_success = () => exception.ShouldBeNull();

        class FakeCommand : CommandBase
        {
            public string Name { get; set; }

            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        class FakeCommand2 : CommandBase
        {
            public string Name2 { get; set; }

            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #region Fake classes

        class FakeMockMessage : CommandBase
        {
            protected override void Execute()
            {
                Dispatcher.Push(new FakeCommand { Name = "Name" });
                Dispatcher.Push(new FakeCommand2 { Name2 = "Name2" });
            }

            #region Override

            public string Id { get; set; }

            public string Id2 { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        static MockMessage<FakeMockMessage, object> mockMessage;

        static Exception exception;

        #endregion
    }
}