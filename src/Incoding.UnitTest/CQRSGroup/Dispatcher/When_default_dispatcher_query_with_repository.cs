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
        Establish establish = () =>
                              {
                                  message = Pleasure.Generator.Invent<QueryWithRepository>(dsl => dsl.GenerateTo(r => r.Setting));

                                  unitOfWorkFactory.Setup(r => r.Create(message.Setting.IsolationLevel.GetValueOrDefault(), false, message.Setting.Connection)).Returns(unitOfWork.Object);
                                  IoCFactory.Instance.StubTryResolveByNamed(message.Setting.DataBaseInstance, unitOfWorkFactory.Object);
                              };

        Because of = () => { result = dispatcher.Query(message); };

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose(), Times.Once());

        It should_be_not_flush = () => unitOfWork.Verify(r => r.Flush(), Times.Never);

        It should_be_read_uncommitted = () => unitOfWorkFactory.Verify(r => r.Create(message.Setting.IsolationLevel.GetValueOrDefault(), false, message.Setting.Connection));

        It should_be_result = () => result.ShouldBeTheSameString();

        #region Establish value

        static QueryWithRepository message;

        static string result;

        #endregion
    }
}