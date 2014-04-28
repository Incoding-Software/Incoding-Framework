namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Behaviors]
    public class Behavior_default_dispatcher_push_with_connection_string
    {
        #region Constructors

        public Behavior_default_dispatcher_push_with_connection_string()
        {            
            unitOfWorkFactory = Pleasure.Mock<IUnitOfWorkFactory>(unitOfWorkFactoryMock =>
                                                                      {
                                                                          unitOfWork = new Mock<IUnitOfWork>();
                                                                          unitOfWorkFactoryMock.Setup(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsAny<string>()))
                                                                                               .Returns(unitOfWork.Object);
                                                                      });

            IoCFactory.Instance.StubTryResolve(unitOfWorkFactory.Object);
            IoCFactory.Instance.StubTryResolve(Pleasure.MockAsObject<IEventBroker>());

            dispatcher = new DefaultDispatcher();
        }

        #endregion

        #region Establish value

        protected static DefaultDispatcher dispatcher;

        static Mock<IUnitOfWorkFactory> unitOfWorkFactory;

        static Mock<IUnitOfWork> unitOfWork;

        public const string connectionString = "Data Source=.;Database=IncRealNhibernateDb;Integrated Security=true;";

        #endregion

        It should_be_commit = () => unitOfWork.Verify(r => r.Flush());

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose());

        It should_be_committed = () => unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, connectionString));
    }
}