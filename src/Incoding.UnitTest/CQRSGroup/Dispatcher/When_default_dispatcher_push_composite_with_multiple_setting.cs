namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_composite_with_multiple_setting : Context_default_dispatcher
    {
        #region Establish value

        static CommandComposite composite;

        static Mock<IUnitOfWorkFactory> specialFactory;

        static Mock<IUnitOfWork> specialUnit;

        static CommandWithRepository commandDef;

        static CommandWithRepository commandSpecial;

        #endregion

        Establish establish = () =>
                              {
                                  commandDef = Pleasure.Generator.Invent<CommandWithRepository>();
                                  commandSpecial = Pleasure.Generator.Invent<CommandWithRepository>(dsl => dsl.GenerateTo(r => r.Setting));

                                  specialFactory = Pleasure.MockStrict<IUnitOfWorkFactory>(unitOfWorkFactoryMock =>
                                                                                           {
                                                                                               var repository = Pleasure.MockAsObject<IRepository>();
                                                                                               specialUnit = Pleasure.Mock<IUnitOfWork>(mock => mock.Setup(s => s.GetRepository()).Returns(repository));
                                                                                               unitOfWorkFactoryMock.Setup(r => r.Create(commandSpecial.Setting.IsolationLevel.GetValueOrDefault(), true, commandSpecial.Setting.Connection)).Returns(specialUnit.Object);
                                                                                           });
                                  IoCFactory.Instance.StubTryResolveByNamed(commandSpecial.Setting.DataBaseInstance, specialFactory.Object);
                              };

        Because of = () => dispatcher.Push(commandComposite =>
                                           {
                                               commandComposite.Quote(commandDef);
                                               commandComposite.Quote(commandSpecial);
                                               commandComposite.Quote(commandSpecial);
                                           });

        It should_be_get_repository = () =>
                                      {
                                          unitOfWork.Verify(r => r.GetRepository(), Times.Once());
                                          specialUnit.Verify(r => r.GetRepository(), Times.Exactly(2));
                                      };
        
        It should_be_factory_create = () =>
                                      {
                                          unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, true, Pleasure.MockIt.IsNull<string>()), Times.Once());
                                          specialFactory.Verify(r => r.Create(commandSpecial.Setting.IsolationLevel.GetValueOrDefault(), true, commandSpecial.Setting.Connection), Times.Once());
                                      };

        It should_be_def_disposable = () =>
                                      {
                                          unitOfWork.Verify(r => r.Dispose(), Times.Once());
                                          specialUnit.Verify(r => r.Dispose(), Times.Once());
                                      };

        It should_be_flush = () =>
                             {
                                 unitOfWork.Verify(r => r.Flush(), Times.Once());
                                 specialUnit.Verify(r => r.Flush(), Times.AtLeast(2));
                             };

        It should_be_special_disposable = () => specialUnit.Verify(r => r.Dispose(), Times.Once());

        It should_be_publish_after_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterExecuteEvent>()));

        It should_be_publish_before_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnBeforeExecuteEvent>()));

        It should_be_publish_complete = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnCompleteExecuteEvent>()));

        It should_not_be_publish_after_fail_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>()), Times.Never());
    }
}