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
    public class When_default_dispatcher_query_with_repository_and_setting : Context_default_dispatcher
    {
        #region Establish value

        static QueryWithRepository message;

        static string result;

        static MessageExecuteSetting setting;

        #endregion

        Establish establish = () =>
                              {
                                  message = Pleasure.Generator.Invent<QueryWithRepository>(dsl => dsl.GenerateTo(r => r.Setting));
                                  setting = Pleasure.Generator.Invent<MessageExecuteSetting>();
                                  unitOfWorkFactory.Setup(r => r.Create(setting.IsolationLevel.GetValueOrDefault(), false, setting.Connection)).Returns(unitOfWork.Object);
                                  IoCFactory.Instance.StubTryResolveByNamed(setting.DataBaseInstance, unitOfWorkFactory.Object);
                              };

        Because of = () =>
                     {
                         result = dispatcher.Query(message, r =>
                                                            {
                                                                r.Connection = setting.Connection;
                                                                r.DataBaseInstance = setting.DataBaseInstance;
                                                                r.IsolationLevel = setting.IsolationLevel;
                                                            });
                     };

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose(), Times.Once());

        It should_be_flush = () => unitOfWork.Verify(r => r.Flush(), Times.Never);
        
        It should_be_read_uncommitted = () => unitOfWorkFactory.Verify(r => r.Create(setting.IsolationLevel.GetValueOrDefault(), false, setting.Connection));

        It should_be_result = () => result.ShouldBeTheSameString();
        
    }
}