namespace Incoding.UnitTest.MSpecGroup
{
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using NCrunch.Framework;

    [Subject(typeof(MockMessage<,>)),Isolated]
    public class When_mock_message_stub_query_new : Behavior_mock_message_stub_query<When_mock_message_stub_query_new.FakeMockMessage, When_mock_message_stub_query_new.FakeMockMessage2, When_mock_message_stub_query_new.FakeMockMessage3>
    {
        Behaves_like<Behavior_mock_message_stub_query<FakeMockMessage, FakeMockMessage2, FakeMockMessage3>> verify;

        #region Fake classes

        public class FakeMockMessage : CommandBase, IIdCommand
        {
            #region Override

            public string Id { get; set; }

            #endregion

            protected override void Execute()
            {
                Result = Dispatcher.New().Query(new FakeQuery { Id = Id });
            }
        }

        public class FakeMockMessage2 : CommandBase
        {
            protected override void Execute()
            {
                Result = Dispatcher.New().Query(new FakeQueryWithoutProperties());
            }
        }

        public class FakeMockMessage3 : CommandBase
        {
            protected override void Execute()
            {
                Result = Dispatcher.New().Query(new FakeQuery { Id = "1" }) + Dispatcher.New().Query(new FakeQuery { Id = "2" });
            }
        }

        #endregion
    }
}