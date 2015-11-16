namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System.Threading.Tasks;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_stub_query_async : Behavior_mock_message_stub_query<When_mock_message_stub_query_async.FakeMockMessage, When_mock_message_stub_query_async.FakeMockMessage2, When_mock_message_stub_query_async.FakeMockMessage3>
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
                Task<string> result = Dispatcher.Async().Query(new FakeQuery { Id = Id });
                result.Wait();
                Result = result.Result;
            }
        }

        public class FakeMockMessage2 : CommandBase
        {
            protected override void Execute()
            {
                Task<string> result = Dispatcher.Async().Query(new FakeQueryWithoutProperties());
                result.Wait();
                Result = result.Result;
            }
        }

        public class FakeMockMessage3 : CommandBase
        {
            protected override void Execute()
            {
                Task<string> result1 = Dispatcher.Async().Query(new FakeQuery { Id = "1" });
                Task<string> result2 = Dispatcher.Async().Query(new FakeQuery { Id = "2" });
                result2.Wait();
                result1.Wait();
                Result = result1.Result + result2.Result;
            }
        }

        #endregion
    }
}