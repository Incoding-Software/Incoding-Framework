namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Data;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_composite_with_exception : Context_default_dispatcher
    {
        #region Establish value

        static CommandComposite composite;

        static Exception exception;

        #endregion

        Establish establish = () =>
                              {
                                  composite = new CommandComposite();
                                  composite.Quote(Pleasure.Generator.Invent<CommandWithRepository>());
                                  composite.Quote(Pleasure.Generator.Invent<CommandWithThrowAndRepository>());
                              };

        Because of = () => { exception = Catch.Exception(() => dispatcher.Push(composite)); };

        It should_be_throw = () => exception.ShouldNotBeNull();

        It should_be_flush = () => unitOfWork.Verify(r => r.Flush(), Times.AtLeast(1));
        
        It should_be_dispose = () => unitOfWork.Verify(r => r.Dispose(), Times.Once());

        It should_be_create = () => unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, true, Pleasure.MockIt.IsNull<string>()), Times.Once());
    }
}