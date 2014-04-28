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

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_real_command
    {
        #region Fake classes

        class FakeCommand : CommandBase
        {
            public override void Execute() { }
        }

        #endregion

        #region Establish value

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
                                                                                                          unitOfWorkFactoryMock.Setup(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<string>())).Returns(unitOfWork.Object);
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

        It should_be_set_unit_of_work = () => message.Setting.UnitOfWork
                                                     .ShouldNotBeNull();

        It should_be_set_data_base_instance = () => message.Setting.DataBaseInstance.ShouldBeTheSameString();
    }
}