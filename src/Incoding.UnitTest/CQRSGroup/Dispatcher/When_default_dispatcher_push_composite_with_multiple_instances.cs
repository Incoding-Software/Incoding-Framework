namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_composite_with_multiple_instances
    {
        #region Establish value

        protected static Mock<IUnitOfWork> defUnit;

        protected static DefaultDispatcher dispatcher;

        protected static Mock<IEventBroker> eventBroker;

        protected static Mock<IUnitOfWorkFactory> defFactory;

        static CommandComposite composite;

        static Mock<IUnitOfWorkFactory> specialFactory;

        static Mock<IUnitOfWork> specialUnit;

        #endregion

        Establish establish = () =>
                                  {
                                      defFactory = Pleasure.MockStrict<IUnitOfWorkFactory>(unitOfWorkFactoryMock =>
                                                                                               {
                                                                                                   defUnit = new Mock<IUnitOfWork>();
                                                                                                   unitOfWorkFactoryMock.Setup(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<string>())).Returns(defUnit.Object);
                                                                                               });
                                      IoCFactory.Instance.StubTryResolve(defFactory.Object);

                                      specialFactory = Pleasure.MockStrict<IUnitOfWorkFactory>(unitOfWorkFactoryMock =>
                                                                                                   {
                                                                                                       specialUnit = new Mock<IUnitOfWork>();
                                                                                                       unitOfWorkFactoryMock.Setup(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<string>())).Returns(specialUnit.Object);
                                                                                                   });
                                      IoCFactory.Instance.StubTryResolveByNamed(Pleasure.Generator.TheSameString(), specialFactory.Object);

                                      eventBroker = Pleasure.Mock<IEventBroker>();
                                      IoCFactory.Instance.StubTryResolve(eventBroker.Object);

                                      dispatcher = new DefaultDispatcher();
                                  };

        Because of = () =>
                         {
                             composite = new CommandComposite();
                             composite.Quote(Pleasure.MockAsObject<CommandBase>());
                             var messageExecuteSetting = new MessageExecuteSetting
                                                             {
                                                                     DataBaseInstance = Pleasure.Generator.TheSameString()
                                                             };
                             composite.Quote(Pleasure.MockAsObject<CommandBase>(), messageExecuteSetting);
                             composite.Quote(Pleasure.MockAsObject<CommandBase>(), messageExecuteSetting);

                             dispatcher.Push(composite);
                         };

        It should_be_def_lush = () => defUnit.Verify(r => r.Flush(), Times.Once());

        It should_be_special_flush = () => specialUnit.Verify(r => r.Flush(), Times.AtLeast(2));

        It should_be_def_commit = () => defUnit.Verify(r => r.Commit(), Times.Once());

        It should_be_special_commit = () => specialUnit.Verify(r => r.Commit(), Times.Once());

        It should_be_def_disposable = () => defUnit.Verify(r => r.Dispose(), Times.Once());

        It should_be_special_disposable = () => specialUnit.Verify(r => r.Dispose(), Times.Once());

        It should_be_def_create = () => defFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<string>()));

        It should_be_special_create = () => specialFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<string>()));

        It should_be_publish_before_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnBeforeExecuteEvent>()));

        It should_be_publish_after_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterExecuteEvent>()));

        It should_be_publish_complete = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnCompleteExecuteEvent>()));

        It should_not_be_publish_after_fail_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>()), Times.Never());
    }
}