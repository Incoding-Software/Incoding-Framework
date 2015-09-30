namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push : Context_default_dispatcher
    {
        #region Establish value

        static Mock<CommandBase> message;

        #endregion

        Establish establish = () => { message = Pleasure.Mock<CommandBase>(); };

        Because of = () => dispatcher.Push(message.Object);

        It should_be_execute = () => message.Verify(r => r.OnExecute(dispatcher, unitOfWork.Object), Times.Once());

        It should_be_flush = () => unitOfWork.Verify(r => r.Flush(), Times.Once());

        It should_be_commit = () => unitOfWork.Verify(r => r.Commit(), Times.Once());

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose());

        It should_be_committed = () => unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<string>()));

        It should_be_publish_before_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnBeforeExecuteEvent>()));

        It should_be_publish_after_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterExecuteEvent>()));

        It should_be_publish_complete = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.Is<OnCompleteExecuteEvent>(@event => @event.Message.ShouldNotBeNull())));

        It should_not_be_publish_after_fail_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>()), Times.Never());
    }
}