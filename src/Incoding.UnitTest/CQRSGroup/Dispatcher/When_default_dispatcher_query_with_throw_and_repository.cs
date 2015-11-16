namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_query_with_throw_and_repository : Context_default_dispatcher
    {
        #region Establish value

        static QueryWithThrowAndRepository message;

        static Exception exception;

        #endregion

        Establish establish = () => { message = Pleasure.Generator.Invent<QueryWithThrowAndRepository>(); };

        Because of = () => { exception = Catch.Exception(() => dispatcher.Query(message)); };
        
        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose(), Times.Once());

        It should_be_not_flush = () => unitOfWork.Verify(r => r.Flush(), Times.Never());
    }
}