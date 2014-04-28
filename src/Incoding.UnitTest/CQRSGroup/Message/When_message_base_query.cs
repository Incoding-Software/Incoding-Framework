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
    public class When_message_base_query
    {
        #region Fake classes

        class FakeQuery : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        class FakeMessage : MessageBase<string>
        {
            public override void Execute()
            {
                string test = Dispatcher.Query(new FakeQuery());
                Result = test;
            }
        }

        #endregion

        #region Establish value

        static FakeMessage message;

        static Mock<IDispatcher> dispatcher;

        #endregion

        Establish establish = () =>
                                  {
                                      message = Pleasure.Generator.Invent<FakeMessage>(dsl => dsl.GenerateTo(r => r.Setting, inventFactoryDsl => inventFactoryDsl.GenerateTo(r => r.Delay)
                                                                                                                                                                 .Tuning(r => r.UnitOfWork, Pleasure.MockAsObject<IUnitOfWork>())));

                                      dispatcher = Pleasure.Mock<IDispatcher>();
                                      dispatcher.StubQuery(new FakeQuery(), Pleasure.Generator.TheSameString(), new MessageExecuteSetting
                                                                                                                    {
                                                                                                                            UnitOfWork = message.Setting.UnitOfWork,
                                                                                                                    });
                                      IoCFactory.Instance.StubTryResolve(dispatcher.Object);
                                  };

        Because of = () => message.Execute();

        It should_be_result = () => message.Result.ShouldBeTheSameString();
    }
}