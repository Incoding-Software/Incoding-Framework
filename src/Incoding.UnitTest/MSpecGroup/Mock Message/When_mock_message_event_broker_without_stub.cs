namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_event_broker_without_stub : Context_mock_message_event_broker
    {
        Establish establish = () =>
                                  {
                                      mockMessage = MockCommand<FakeCommand>
                                              .When(new FakeCommand());
                                  };

        Because of = () => { exception = Catch.Exception(() => mockMessage.Original.Execute()); };

        It should_not_be_exception = () => exception.ShouldNotBeNull();

        It should_be_verify_publish = () => { mockMessage.ShouldBePublished(); };
    }
}