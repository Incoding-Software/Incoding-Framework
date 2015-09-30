namespace Incoding.UnitTest
{
    using System;
    using System.Data;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_composite_with_exception : Context_default_dispatcher
    {
        #region Establish value

        static CommandComposite composite;

        #endregion

        #region Establish value

        static Exception exception;

        #endregion

        Establish establish = () =>
                                  {
                                      composite = new CommandComposite();
                                      composite.Quote(Pleasure.MockAsObject<CommandBase>());
                                      composite.Quote(Pleasure.MockAsObject<CommandBase>(mock => mock.Setup(r => r.OnExecute(dispatcher, unitOfWork.Object)).Throws<SpecificationException>()));
                                  };

        Because of = () => { exception = Catch.Exception(() => dispatcher.Push(composite)); };

        It should_be_throw = () => exception.ShouldBeOfType<SpecificationException>();

        It should_be_flush = () => unitOfWork.Verify(r => r.Flush(), Times.AtLeast(1));

        It should_be_commit = () => unitOfWork.Verify(r => r.Commit(), Times.Never());

        It should_be_dispose = () => unitOfWork.Verify(r => r.Dispose(), Times.Once());

        It should_be_create = () => unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<string>()), Times.Once());
    }
}