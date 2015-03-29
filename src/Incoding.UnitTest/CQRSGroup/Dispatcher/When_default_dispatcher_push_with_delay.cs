namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.Block;
    using Incoding.Block.ExceptionHandling;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_with_delay : Context_default_dispatcher
    {
        #region Fake classes

        class FakeCommand : CommandBase
        {
            // ReSharper disable UnusedMember.Local

            #region Properties

            public string Prop1 { get; set; }

            #endregion

            // ReSharper restore UnusedMember.Local
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static CommandBase message;

        static Mock<IDispatcher> mockDispatcher;

        #endregion

        Establish establish = () =>
                                  {
                                      message = Pleasure.Generator.Invent<FakeCommand>();

                                      mockDispatcher = Pleasure.Mock<IDispatcher>();
                                      IoCFactory.Instance.StubTryResolve(mockDispatcher.Object);
                                  };

        Because of = () => dispatcher.Push(message, Pleasure.Generator.Invent<MessageExecuteSetting>(dsl => { }));

        It should_be_push_add_delay_to_scheduler = () => mockDispatcher.ShouldBePush<AddDelayToSchedulerCommand>(command =>
                                                                                                                     {
                                                                                                                         command.Commands.Count.ShouldEqual(1);
                                                                                                                         command.Commands[0].ShouldEqualWeak(message);
                                                                                                                     });

        It should_be_flush = () => unitOfWork.Verify(r => r.Flush(), Times.Never());

        It should_be_commit = () => unitOfWork.Verify(r => r.Commit(), Times.Never());

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose(), Times.Never());
    }
}