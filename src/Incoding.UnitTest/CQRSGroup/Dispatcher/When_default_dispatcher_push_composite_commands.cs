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
    public class When_default_dispatcher_push_composite_commands : Context_default_dispatcher
    {
        #region Establish value

        static Mock<CommandBase> message;

        static Mock<CommandBase> message2;

        static CommandComposite composite;

        #endregion

        Establish establish = () =>
                                  {
                                      message = Pleasure.Mock<CommandBase>();
                                      message2 = Pleasure.Mock<CommandBase>();

                                      composite = new CommandComposite();
                                      composite.Quote(message.Object);
                                      composite.Quote(message2.Object);
                                  };

        Because of = () => dispatcher.Push(composite);

        It should_be_execute_message_1 = () => message.Verify(r => r.Execute(), Times.Once());

        It should_be_execute_message_2 = () => message2.Verify(r => r.Execute(), Times.Once());

        It should_be_flush = () => unitOfWork.Verify(r => r.Flush(), Times.AtLeast(2));

        It should_be_commit = () => unitOfWork.Verify(r => r.Commit(), Times.Once());

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose(), Times.Once());

        It should_be_committed = () => unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<string>()));

        It should_be_publish_before_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnBeforeExecuteEvent>()));

        It should_be_publish_after_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterExecuteEvent>()));

        It should_be_publish_complete = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnCompleteExecuteEvent>()));

        It should_not_be_publish_after_fail_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>()), Times.Never());
    }
}