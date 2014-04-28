namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Data;
    using Incoding.Block;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_composite_complex : Context_default_dispatcher
    {
        #region Fake classes

        public class FakeDelayCommand : CommandBase
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static FakeDelayCommand delayCommand;

        static Mock<CommandBase> command;

        static Mock<QueryBase<string>> query;

        static CommandComposite composite;

        static Mock<IDispatcher> delayDispatcher;

        #endregion

        Establish establish = () =>
                                  {
                                      delayDispatcher = Pleasure.Mock<IDispatcher>();
                                      IoCFactory.Instance.StubTryResolve(delayDispatcher.Object);

                                      command = Pleasure.Mock<CommandBase>();
                                      delayCommand = Pleasure.Generator.Invent<FakeDelayCommand>();
                                      query = Pleasure.Mock<QueryBase<string>>();

                                      composite = new CommandComposite();
                                      composite.Quote(delayCommand, new MessageExecuteSetting
                                                                        {
                                                                                Delay = Pleasure.Generator.Invent<MessageDelaySetting>()
                                                                        });
                                      composite.Quote(delayCommand, new MessageExecuteSetting
                                                                        {
                                                                                Delay = Pleasure.Generator.Invent<MessageDelaySetting>()
                                                                        });
                                      composite.Quote(command.Object);
                                      composite.Quote(query.Object);
                                  };

        Because of = () => dispatcher.Push(composite);

        It should_be_execute_command = () => command.Verify(r => r.Execute(), Times.Once());

        It should_be_execute_query = () => query.Verify(r => r.Execute(), Times.Once());

        It should_be_flush = () => unitOfWork.Verify(r => r.Flush(), Times.AtLeast(1));

        It should_be_commit = () => unitOfWork.Verify(r => r.Commit(), Times.Once());

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose(), Times.Once());

        It should_be_committed = () => unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<string>()));

        It should_be_publish_before_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnBeforeExecuteEvent>()));

        It should_be_publish_after_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterExecuteEvent>()));

        It should_be_publish_complete = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnCompleteExecuteEvent>()));

        It should_not_be_publish_after_fail_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>()), Times.Never());

        It should_be_push_add_delay_to_scheduler = () => delayDispatcher.ShouldBePush<AddDelayToSchedulerCommand>(schedulerCommand =>
                                                                                                                      {
                                                                                                                          schedulerCommand.ShouldNotBeNull();
                                                                                                                          schedulerCommand.Commands.Count.ShouldEqual(2);
                                                                                                                          schedulerCommand.Commands[0].ShouldEqualWeak(delayCommand);
                                                                                                                          schedulerCommand.Commands[1].ShouldEqualWeak(delayCommand);
                                                                                                                      });
    }
}