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
    public class When_default_dispatcher_push_with_throw_exception : Context_default_dispatcher
    {
        #region Establish value

        static Mock<CommandBase> message;

        static Exception exception;

        #endregion

        Establish establish = () => { message = Pleasure.Mock<CommandBase>(mock => mock.Setup(r => r.OnExecute(dispatcher, unitOfWork.Object)).Throws<ArgumentException>()); };

        Because of = () => { exception = Catch.Exception(() => dispatcher.Push(message.Object)); };

        It should_be_exception = () => exception.ShouldBeOfType<ArgumentException>();

        It should_be_execute = () => message.Verify(r => r.OnExecute(dispatcher, unitOfWork.Object), Times.Once());

        It should_be_not_commit = () => unitOfWork.Verify(r => r.Commit(), Times.Never());

        It should_be_dispose = () => unitOfWork.Verify(r => r.Dispose(), Times.Once());
    }
}