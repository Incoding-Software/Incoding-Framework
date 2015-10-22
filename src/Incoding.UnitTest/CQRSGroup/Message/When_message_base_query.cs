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

        class FakeMessage : MessageBase
        {
            protected override void Execute()
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
                                  dispatcher = Pleasure.Mock<IDispatcher>();
                                  var setting = Pleasure.Generator.Invent<MessageExecuteSetting>();
                                  dispatcher.StubQuery(new FakeQuery(), Pleasure.Generator.TheSameString(), setting);
                                  message = Pleasure.Generator.Invent<FakeMessage>(dsl => dsl.GenerateTo(r => r.Setting, factoryDsl =>
                                                                                                                         factoryDsl.Tuning(r => r.Connection, setting.Connection)
                                                                                                                                   .Tuning(r => r.IsolationLevel, setting.IsolationLevel)
                                                                                                                                   .Tuning(r => r.DataBaseInstance, setting.DataBaseInstance)));
                              };

        Because of = () => message.OnExecute(dispatcher.Object, new Lazy<IUnitOfWork>(() => Pleasure.MockStrictAsObject<IUnitOfWork>()));

        It should_be_result = () => message.Result.ShouldEqual(Pleasure.Generator.TheSameString());
    }
}