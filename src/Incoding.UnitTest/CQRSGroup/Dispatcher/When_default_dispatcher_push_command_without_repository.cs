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
    public class When_default_dispatcher_push_command_without_repository : Context_default_dispatcher
    {
        #region Establish value

        static CommandWithoutRepository message;

        #endregion

        Establish establish = () => { message = Pleasure.Generator.Invent<CommandWithoutRepository>(); };

        Because of = () => dispatcher.Push(message);
        
        It should_be_committed = () => unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, true, Pleasure.MockIt.IsNull<string>()), Times.Never());

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose(), Times.Never());

        It should_be_flush = () => unitOfWork.Verify(r => r.Flush(), Times.Never());
    }
}