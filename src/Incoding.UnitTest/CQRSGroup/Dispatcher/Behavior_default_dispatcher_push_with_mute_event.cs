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

    [Behaviors]
    public class Behavior_default_dispatcher_push_with_mute_event : Context_default_dispatcher
    {
        #region Static Fields

        protected static Exception exception;

        #endregion

        #region Fields

        It should_be_exception = () => exception.ShouldNotBeNull();

        It should_be_not_publish_before_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnBeforeExecuteEvent>()), Times.Never());

        It should_be_not_publish_after_fail = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>()), Times.Never());

        It should_be_not_publish_complete = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnCompleteExecuteEvent>()), Times.Never());

        #endregion
    }
}