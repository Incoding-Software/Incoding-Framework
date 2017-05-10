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
                                     mockMessage.Execute();
                                 };

        It should_be_stub_push_with_result_provided = () =>
                                 {
                                     var input = Pleasure.Generator.Invent<FakeMockMessageWithResult>();
                                     var result = Pleasure.Generator.String();
                                     var mockMessage = MockCommand<FakeMockMessageWithResult>
                                             .When(input)
                                             .StubPush(new FakeMockMessageWithResult
                                                       {
                                                               Id = Pleasure.Generator.TheSameString()
                                                       }, result: result
                                                       )
                                             .StubPush(new FakeMockMessage {Id = result});

                                     mockMessage.Execute();
                                 };

        It should_be_stub_push_double_same_command = () =>
                                                     {
                                                         var input = Pleasure.Generator.Invent<FakeMockMessageWithDoublePushSameCommand>();
                                                         var mockMessage = MockCommand<FakeMockMessageWithDoublePushSameCommand>
                                                                 .When(input)
                                                                 .StubPush<FakeMockMessage>(dsl => dsl.Tuning(r => r.Id, "2"))
                                                                 .StubPush<FakeMockMessage>(dsl => dsl.Tuning(r => r.Id, "1"));
                                                         mockMessage.Execute();
                                                     };

        It should_be_stub_push_invent = () =>
                                        {
                                            var input = Pleasure.Generator.Invent<FakeMockMessage>();
                                            var mockMessage = MockCommand<FakeMockMessage>
                                                    .When(input)
                                                    .StubPush<FakeMockMessage>(dsl => dsl.Tuning(r => r.Id, Pleasure.Generator.TheSameString()));
                                            mockMessage.Execute();
                                        };

        It should_be_stub_push_with_result = () =>
                                             {
                                                 var value = "123";
                                                 var input = Pleasure.Generator.Invent<Fake2MockMessage>(r => r.Tuning(s => s.Result, value));
                                                 var mockMessage = MockCommand<Fake2MockMessage>
                                                         .When(input)
                                                         .StubPush(new Fake2MockMessage
                                                                   { 
                                                                           Id = Pleasure.Generator.TheSameString()
                                                                   });
                                                 mockMessage.Execute();
                                                 mockMessage.ShouldBeIsResult(value);
                                             };

        It should_be_stub_push_without_push_ = () =>
                                               {
                                                   var input = Pleasure.Generator.Invent<FakeMockMessage>();
                                                   var mockMessage = MockCommand<FakeMockMessage>
                                                           .When(input)
                                                           .StubPush(new FakeMockMessage
                                                                     {
                                                                             Id = Pleasure.Generator.TheSameString().Inverse()
                                                                     });
                                                   Catch.Exception(mockMessage.Execute).ShouldNotBeNull();
                                               };

        It should_be_stub_push_without_push_multiple = () =>
                                                       {
                                                           var input = Pleasure.Generator.Invent<FakeMockMessage>();
                                                           var mockMessage = MockCommand<FakeMockMessage>
                                                                   .When(input)
                                                                   .StubPush(new FakeMockMessage { Id = Pleasure.Generator.TheSameString().Inverse() })
                                                                   .StubPush(new FakeMockMessage { Id = Pleasure.Generator.TheSameString() });
                                                           Catch.Exception(mockMessage.Execute).Message.ShouldEqual("Not Stub for FakeMockMessage");
                                                       };

        #region Fake classes

        class FakeMockMessage : CommandBase
        {
            #region Override

            public string Id { get; set; }

            #endregion

            protected override void Execute()
            {
                Dispatcher.Push(new FakeMockMessage { Id = Pleasure.Generator.TheSameString() });
            }
        }

        class FakeMockMessageWithResult : CommandBase
        {
            #region Override

            public string Id { get; set; }

            #endregion

            protected override void Execute()
            {
                var fakeMockMessageWithResult = new FakeMockMessageWithResult() { Id = Pleasure.Generator.TheSameString() };
                Dispatcher.Push(fakeMockMessageWithResult);
                Dispatcher.Push(new FakeMockMessage() { Id = (string)fakeMockMessageWithResult.Result });
            }
        }

        class Fake2MockMessage : CommandBase
        {
            #region Override

            public string Id { get; set; }

            #endregion

            protected override void Execute()
            {
                Result = Dispatcher.Push<string>(new Fake2MockMessage { Id = Pleasure.Generator.TheSameString() });
            }
        }

        class FakeMockMessageWithDoublePushSameCommand : CommandBase
        {
            protected override void Execute()
            {
                Dispatcher.Push(new FakeMockMessage { Id = "1" });
                Dispatcher.Push(new FakeMockMessage { Id = "2" });
            }
        }

        #endregion
    }
}