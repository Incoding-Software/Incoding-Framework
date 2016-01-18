namespace Incoding.UnitTest
{
    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_with_throw_and_without_repository : Context_default_dispatcher
    {
        #region Establish value

        static CommandBase message;

        static Exception exception;

        #endregion

        Establish establish = () => { message = Pleasure.Generator.Invent<CommandWithThrow>(); };

        Because of = () => { exception = Catch.Exception(() => dispatcher.Push(message)); };

        It should_be_exception = () => exception.ShouldNotBeNull();
        
        It should_be_not_flush = () => unitOfWork.Verify(r => r.Flush(), Times.Never());

        It should_be_dispose = () => unitOfWork.Verify(r => r.Dispose(), Times.Never());
        
    }
}