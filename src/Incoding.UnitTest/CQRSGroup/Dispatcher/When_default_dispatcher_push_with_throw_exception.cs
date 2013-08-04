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
    public class When_default_dispatcher_push_with_throw_exception : Context_default_dispatcher
    {
        #region Estabilish value

        static Mock<CommandBase> message;

        static Exception exception;

        #endregion

        Establish establish = () =>
                                  {
                                      message = Pleasure.Mock<CommandBase>(mock => mock.Setup(r => r.Execute()).Throws<ArgumentException>());
                                      eventBroker.Setup(r => r.HasSubscriber(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>())).Returns(true);
                                  };

        Because of = () => { exception = Catch.Exception(() => dispatcher.Push(message.Object)); };

        It should_be_exception = () => exception.ShouldBeNull();

        It should_be_execute = () => message.Verify(r => r.Execute(), Times.Once());

        It should_be_publish_before_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnBeforeExecuteEvent>()));

        It should_be_publish_after_fail = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>()));

        It should_be_publish_complete = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnCompleteExecuteEvent>()));
    }
}