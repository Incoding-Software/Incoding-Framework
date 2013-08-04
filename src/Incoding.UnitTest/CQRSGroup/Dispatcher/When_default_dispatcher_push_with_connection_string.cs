namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using System.Data.SqlClient;
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
    public class When_default_dispatcher_push_with_connection_string
    {
        #region Fake classes

        class FakeCommand : CommandBase
        {
            public override void Execute() { }
        }

        #endregion

        #region Estabilish value

        static FakeCommand message;

        static DefaultDispatcher dispatcher;

        static Mock<IUnitOfWorkFactory> unitOfWorkFactory;

        static Mock<IUnitOfWork> unitOfWork;

        static SqlConnection connectionString;

        #endregion

        Establish establish = () =>
                                  {
                                      connectionString = new SqlConnection(@"Data Source=Work\SQLEXPRESS;Database=IncRealDb;Integrated Security=true;");
                                      unitOfWorkFactory = Pleasure.MockStrict<IUnitOfWorkFactory>(unitOfWorkFactoryMock =>
                                                                                                      {
                                                                                                          unitOfWork = new Mock<IUnitOfWork>();
                                                                                                          unitOfWorkFactoryMock.Setup(r => r.Create(IsolationLevel.ReadCommitted, 
                                                                                                                                                    Pleasure.MockIt.Is<SqlConnection>(s => s.ConnectionString.ShouldEqual(connectionString.ConnectionString))))
                                                                                                                               .Returns(unitOfWork.Object);
                                                                                                      });

                                      IoCFactory.Instance.StubTryResolve(unitOfWorkFactory.Object);
                                      IoCFactory.Instance.StubTryResolve(Pleasure.MockAsObject<IEventBroker>());

                                      dispatcher = new DefaultDispatcher();
                                      message = new FakeCommand();
                                  };

        Because of = () => dispatcher.Push(message, new MessageExecuteSetting
                                                        {
                                                                Connection = connectionString
                                                        });

        It should_be_commit = () => unitOfWork.Verify(r => r.Commit());

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose());

        It should_be_committed = () =>
                                 unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.Is<SqlConnection>(sqlConnection => sqlConnection.ConnectionString.ShouldEqual(connectionString.ConnectionString))));
    }
}