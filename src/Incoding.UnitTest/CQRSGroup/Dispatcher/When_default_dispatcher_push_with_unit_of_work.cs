namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_with_unit_of_work : Context_default_dispatcher
    {
        #region Fake classes

        class FakeMessage : CommandBase
        {
            public override void Execute() { }
        }

        #endregion

        #region Establish value

        static FakeMessage message;

        static Mock<IUnitOfWork> existUnitOfWork;

        #endregion

        Establish establish = () =>
                                  {
                                      message = new FakeMessage();
                                      existUnitOfWork = Pleasure.Mock<IUnitOfWork>(mock => mock.Setup(r => r.IsOpen()).Returns(true));
                                  };

        Because of = () => dispatcher.Push(message, new MessageExecuteSetting
                                                        {
                                                                UnitOfWork = existUnitOfWork.Object
                                                        });

        It should_be_factory_create = () => unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<string>()), Times.Never());

        It should_be_flush = () => existUnitOfWork.Verify(r => r.Flush(), Times.Once());

        It should_be_not_commit = () => existUnitOfWork.Verify(r => r.Commit(), Times.Never());

        It should_be_not_dispose = () => existUnitOfWork.Verify(r => r.Dispose(), Times.Never());

        It should_be_publish_before_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnBeforeExecuteEvent>()));

        It should_be_publish_after_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterExecuteEvent>()));

        It should_be_publish_complete = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.Is<OnCompleteExecuteEvent>(@event => @event.Message.ShouldNotBeNull())));

        It should_not_be_publish_after_fail_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>()), Times.Never());
    }
}