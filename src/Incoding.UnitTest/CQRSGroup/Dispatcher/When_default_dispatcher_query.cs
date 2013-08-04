namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_query : Context_default_dispatcher
    {
        #region Fake classes

        class FakeQuery : QueryBase<FakeQuery.FakeResponse>
        {
            #region Nested classes

            public class FakeResponse
            {
                #region Properties

                public int Value { get; set; }

                #endregion
            }

            #endregion

            protected override FakeResponse ExecuteResult()
            {
                return new FakeResponse
                           {
                                   Value = 5
                           };
            }
        }

        #endregion

        #region Estabilish value

        static FakeQuery message;

        static FakeQuery.FakeResponse result;

        #endregion

        Establish establish = () => { message = new FakeQuery(); };

        Because of = () => { result = dispatcher.Query(message); };

        It should_be_result = () => result.ShouldEqualWeak(new FakeQuery.FakeResponse { Value = 5 });

        It should_be_not_commit = () => unitOfWork.Verify(r => r.Commit(), Times.Never());

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose());

        It should_be_read_uncommitted = () => unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadUncommitted, Pleasure.MockIt.IsNull<IDbConnection>()));

        It should_be_publish_before_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnBeforeExecuteEvent>()));

        It should_be_publish_after_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterExecuteEvent>()));

        It should_be_publish_complete = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnCompleteExecuteEvent>()));

        It should_not_be_publish_after_fail_execute = () => eventBroker.Verify(r => r.Publish(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>()), Times.Never());
    }
}