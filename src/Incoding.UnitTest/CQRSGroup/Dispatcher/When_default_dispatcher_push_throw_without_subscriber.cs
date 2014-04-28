namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_throw_without_subscriber : Context_default_dispatcher
    {
        #region Establish value

        static Mock<CommandBase> message;

        static Exception exception;

        #endregion

        Establish establish = () =>
                                  {
                                      message = Pleasure.Mock<CommandBase>(mock => mock.Setup(r => r.Execute()).Throws<ArgumentException>());
                                      eventBroker.Setup(r => r.HasSubscriber(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>())).Returns(false);
                                  };

        Because of = () => { exception = Catch.Exception(() => dispatcher.Push(message.Object)); };

        It should_be_exception = () => exception.ShouldBeOfType<ArgumentException>();

        It should_be_execute = () => message.Verify(r => r.Execute(), Times.Once());

        It should_be_not_publish_after_fail = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>()), Times.Never());
    }
}