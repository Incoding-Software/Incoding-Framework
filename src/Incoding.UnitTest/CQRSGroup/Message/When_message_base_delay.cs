namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.Block.ExceptionHandling;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(MessageBase<>))]
    public class When_message_base_delay
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
                Dispatcher.Delay(new FakeCommand(), setting =>
                                                        {
                                                            setting.Connection = delaySetting.Connection;
                                                            setting.DataBaseInstance = delaySetting.DataBaseInstance;
                                                            setting.Policy = delaySetting.Policy;
                                                            setting.UID = delaySetting.UID;
                                                        });
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
                                      delaySetting = Pleasure.Generator.Invent<MessageDelaySetting>(dsl => dsl.Tuning(r => r.Policy, ActionPolicy.Repeat(2)));
                                      message = Pleasure.Generator.Invent<FakeMessage>(dsl => dsl.GenerateTo(r => r.Setting, inventFactoryDsl => inventFactoryDsl.Tuning(r => r.UnitOfWork, Pleasure.MockAsObject<IUnitOfWork>())));
                                      dispatcher = Pleasure.Mock<IDispatcher>();
                                      IoCFactory.Instance.StubTryResolve(dispatcher.Object);
                                  };

        Because of = () => message.Execute();

        It should_be_push = () => dispatcher.ShouldBePush(new FakeCommand(), new MessageExecuteSetting
                                                                                 {
                                                                                         Delay = delaySetting
                                                                                 });
    }
}