namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.CQRS;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_with_connection_string : Behavior_default_dispatcher_push_with_connection_string
    {
        #region Fake classes

        class FakeCommand : CommandBase
        {
            protected override void Execute() { }
        }

        #endregion

        Because of = () => dispatcher.Push(new FakeCommand(), new MessageExecuteSetting
                                                                  {
                                                                          Connection = connectionString
                                                                  });

        Behaves_like<Behavior_default_dispatcher_push_with_connection_string> should_be_verify;
    }
}