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

    [Subject(typeof(MessageBase<>))]
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

        class FakeMessage : MessageBase<string>
        {
            protected override void Execute()
            {
                Dispatcher.Push(new FakeCommand(), setting => { });
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
                                  message = Pleasure.Generator.Invent<FakeMessage>(dsl => dsl.GenerateTo(r => r.Setting, inventFactoryDsl => { }));
                                  dispatcher = Pleasure.Mock<IDispatcher>();
                              };

        Because of = () => message.OnExecute(dispatcher.Object, unitOfWork);

        It should_be_push = () => dispatcher.ShouldBePush(new FakeCommand());
    }
}