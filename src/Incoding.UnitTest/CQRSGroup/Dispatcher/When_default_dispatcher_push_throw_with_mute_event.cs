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
    public class When_default_dispatcher_push_throw_with_mute_event : Context_default_dispatcher
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

        Because of = () =>
                         {
                             exception = Catch.Exception(() => dispatcher.Push(message.Object, new MessageExecuteSetting
                                                                                                   {
                                                                                                           PublishEventOnError = false, 
                                                                                                           PublishEventOnBefore = false, 
                                                                                                           PublishEventOnComplete = false
                                                                                                   }));
                         };

        It should_be_exception = () => exception.ShouldNotBeNull();

        It should_be_execute = () => message.Verify(r => r.Execute(), Times.Once());

        It should_be_not_publish_before_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnBeforeExecuteEvent>()), Times.Never());

        It should_be_not_publish_after_fail = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>()), Times.Never());

        It should_be_not_publish_complete = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnCompleteExecuteEvent>()), Times.Never());
    }
}