namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_query_without_repository : Context_default_dispatcher
    {
        Establish establish = () => { message = Pleasure.Generator.Invent<QueryWithoutRepository>(); };

        Because of = () => { result = dispatcher.Query(message); };

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose(), Times.Never());

        It should_be_not_flush = () => unitOfWork.Verify(r => r.Flush(), Times.Never());

        It should_be_publish_after_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterExecuteEvent>()));

        It should_be_publish_before_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnBeforeExecuteEvent>()));

        It should_be_publish_complete = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnCompleteExecuteEvent>()));

        It should_be_result = () => result.ShouldBeTheSameString();

        It should_not_be_publish_after_fail_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>()), Times.Never());

        #region Establish value

        static QueryWithoutRepository message;

        static string result;

        #endregion
    }
}