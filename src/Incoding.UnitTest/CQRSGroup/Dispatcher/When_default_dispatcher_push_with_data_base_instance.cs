namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.Extensions;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_with_data_base_instance
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

        #endregion

        Establish establish = () =>
                                  {
                                      unitOfWorkFactory = Pleasure.MockStrict<IUnitOfWorkFactory>(unitOfWorkFactoryMock =>
                                                                                                      {
                                                                                                          unitOfWork = new Mock<IUnitOfWork>();
                                                                                                          unitOfWorkFactoryMock.Setup(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<IDbConnection>())).Returns(unitOfWork.Object);
                                                                                                      });
                                      IoCFactory.Instance.StubTryResolveByNamed(Pleasure.Generator.TheSameString(), unitOfWorkFactory.Object);

                                      IoCFactory.Instance.StubTryResolve(Pleasure.MockAsObject<IEventBroker>());

                                      dispatcher = new DefaultDispatcher();
                                      message = new FakeCommand();
                                  };

        Because of = () => dispatcher.Push(message, new MessageExecuteSetting
                                                        {
                                                                DataBaseInstance = Pleasure.Generator.TheSameString()
                                                        });

        It should_be_set_configuration_name_to_message = () => message.TryGetValue("dataBaseInstance")
                                                                      .ToString().ShouldBeTheSameString();

        It should_be_commit = () => unitOfWork.Verify(r => r.Commit());

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose());

        It should_be_committed = () => unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<IDbConnection>()));
    }
}