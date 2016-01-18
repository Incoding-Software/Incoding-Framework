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
        
        It should_be_result = () => result.ShouldBeTheSameString();
        
        #region Establish value

        static QueryWithoutRepository message;

        static string result;

        #endregion
    }
}