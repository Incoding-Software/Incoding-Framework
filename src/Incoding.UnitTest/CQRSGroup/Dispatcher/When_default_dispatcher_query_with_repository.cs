namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_query_with_repository : Context_default_dispatcher
    {
        #region Establish value

        static QueryWithRepository message;

        static string result;

        #endregion

        Establish establish = () =>
                              {
                                  message = Pleasure.Generator.Invent<QueryWithRepository>(dsl => dsl.GenerateTo(r => r.Setting));

                                  unitOfWorkFactory.Setup(r => r.Create(message.Setting.IsolationLevel.GetValueOrDefault(), false, message.Setting.Connection)).Returns(unitOfWork.Object);
                                  IoCFactory.Instance.StubTryResolveByNamed(message.Setting.DataBaseInstance, unitOfWorkFactory.Object);
                              };

        Because of = () => { result = dispatcher.Query(message); };
        

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose(), Times.Once());

        It should_be_not_flush = () => unitOfWork.Verify(r => r.Flush(), Times.Once());

        It should_be_publish_after_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterExecuteEvent>()));

        It should_be_publish_before_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnBeforeExecuteEvent>()));

        It should_be_publish_complete = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnCompleteExecuteEvent>()));

        It should_be_read_uncommitted = () => unitOfWorkFactory.Verify(r => r.Create(message.Setting.IsolationLevel.GetValueOrDefault(), false, message.Setting.Connection));

        It should_be_result = () => result.ShouldBeTheSameString();

        It should_not_be_publish_after_fail_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>()), Times.Never());
    }
}