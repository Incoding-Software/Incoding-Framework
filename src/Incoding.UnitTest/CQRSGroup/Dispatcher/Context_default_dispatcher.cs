namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.MSpecContrib;
    using Moq;

    #endregion

    public class Context_default_dispatcher
    {
        #region Static Fields

        protected static Mock<IUnitOfWork> unitOfWork;

        protected static DefaultDispatcher dispatcher;

        protected static Mock<IEventBroker> eventBroker;

        protected static Mock<IUnitOfWorkFactory> unitOfWorkFactory;

        #endregion

        #region Constructors

        protected Context_default_dispatcher()
        {
            unitOfWorkFactory = Pleasure.MockStrict<IUnitOfWorkFactory>(unitOfWorkFactoryMock =>
                                                                            {
                                                                                unitOfWork = new Mock<IUnitOfWork>();
                                                                                unitOfWorkFactoryMock.Setup(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<string>())).Returns(unitOfWork.Object);
                                                                                unitOfWorkFactoryMock.Setup(r => r.Create(IsolationLevel.ReadUncommitted, Pleasure.MockIt.IsNull<string>())).Returns(unitOfWork.Object);
                                                                            });
            IoCFactory.Instance.StubTryResolve(unitOfWorkFactory.Object);

            eventBroker = Pleasure.Mock<IEventBroker>();
            IoCFactory.Instance.StubTryResolve(eventBroker.Object);

            dispatcher = new DefaultDispatcher();
        }

        #endregion
    }
}