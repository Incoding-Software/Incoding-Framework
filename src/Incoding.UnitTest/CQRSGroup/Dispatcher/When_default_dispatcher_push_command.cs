namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_command : Context_default_dispatcher
    {
        #region Establish value

        static CommandWithRepository message;

        #endregion

        private static Mock<IMessageInterception> messageInterception;

        Establish establish = () =>
                              {
                                  messageInterception = Pleasure.Mock<IMessageInterception>();
                                  DefaultDispatcher.SetInterception(() => messageInterception.Object);
                                  message = Pleasure.Generator.Invent<CommandWithRepository>();
                              };

        Because of = () => dispatcher.Push(message);

        It should_be_committed = () => unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, true, Pleasure.MockIt.IsNull<string>()), Times.Once());

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose(), Times.Once());

        It should_be_flush = () => unitOfWork.Verify(r => r.Flush(), Times.Once());

        It should_be_interception_on_after = () => messageInterception.Verify(s => s.OnAfter(Pleasure.MockIt.IsAny<CommandWithRepository>()));

        It should_be_interception_on_before = () => messageInterception.Verify(s => s.OnBefore(Pleasure.MockIt.IsAny<CommandWithRepository>()));
    }
}