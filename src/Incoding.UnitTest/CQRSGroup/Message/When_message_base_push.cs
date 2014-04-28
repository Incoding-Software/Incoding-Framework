namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(MessageBase<>))]
    public class When_message_base_push
    {
        #region Fake classes

        class FakeCommand : CommandBase
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        class FakeMessage : MessageBase<string>
        {
            public override void Execute()
            {
                Dispatcher.Push(new FakeCommand(), setting => setting.Delay = delaySetting);
            }
        }

        #endregion

        #region Establish value

        static FakeMessage message;

        static Mock<IDispatcher> dispatcher;

        static MessageDelaySetting delaySetting;

        #endregion

        Establish establish = () =>
                                  {
                                      delaySetting = Pleasure.Generator.Invent<MessageDelaySetting>();
                                      var unitOfWork = Pleasure.MockAsObject<IUnitOfWork>(mock => mock.Setup(r => r.IsOpen()).Returns(true));
                                      message = Pleasure.Generator.Invent<FakeMessage>(dsl => dsl.GenerateTo(r => r.Setting, inventFactoryDsl => inventFactoryDsl.Tuning(r => r.UnitOfWork, unitOfWork)));
                                      dispatcher = Pleasure.Mock<IDispatcher>();
                                      IoCFactory.Instance.StubTryResolve(dispatcher.Object);
                                  };

        Because of = () => message.Execute();

        It should_be_push = () => dispatcher.ShouldBePush(new FakeCommand(), new MessageExecuteSetting
                                                                                 {
                                                                                         UnitOfWork = message.Setting.UnitOfWork, 
                                                                                         Delay = delaySetting
                                                                                 });
    }
}