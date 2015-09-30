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
    public class When_message_base_dispatcher_query
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
                                  string connection = Pleasure.Generator.String();
                                  string dbInstance = Pleasure.Generator.String();
                                  dispatcher = Pleasure.Mock<IDispatcher>();
                                  dispatcher.StubQuery(new FakeQuery(), Pleasure.Generator.TheSameString(), new MessageExecuteSetting
                                                                                                            {
                                                                                                                    Connection = connection, 
                                                                                                                    DataBaseInstance = dbInstance
                                                                                                            });
                                  message = Pleasure.Generator.Invent<FakeMessage>(dsl => dsl.GenerateTo(r => r.Setting, factoryDsl =>
                                                                                                                         factoryDsl.Tuning(r => r.Connection, connection)
                                                                                                                                   .Tuning(r => r.DataBaseInstance, dbInstance)));
                              };

        Because of = () => message.OnExecute(dispatcher.Object, Pleasure.MockStrictAsObject<IUnitOfWork>());

        It should_be_result = () => message.Result.ShouldBeTheSameString();
    }
}