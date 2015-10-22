namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(MessageBase))]
    public class When_message_base_dispatcher_push
    {
        #region Fake classes

        class FakeCommand : CommandBase
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        class FakeMessage : MessageBase
        {
            protected override void Execute()
            {
                Dispatcher.Push(new FakeCommand());
            }
        }

        #endregion

        #region Establish value

        static FakeMessage message;

        static Mock<IDispatcher> dispatcher;

        static IUnitOfWork unitOfWork;

        #endregion

        Establish establish = () =>
                              {
                                  unitOfWork = Pleasure.MockAsObject<IUnitOfWork>();
                                  message = Pleasure.Generator.Invent<FakeMessage>();
                                  dispatcher = Pleasure.Mock<IDispatcher>();
                              };

        Because of = () => message.OnExecute(dispatcher.Object, new Lazy<IUnitOfWork>(() => unitOfWork));

        It should_be_push = () => dispatcher.ShouldBePush(new FakeCommand());
    }
}