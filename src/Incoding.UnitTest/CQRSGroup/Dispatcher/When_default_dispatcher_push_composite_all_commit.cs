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
    public class When_default_dispatcher_push_composite_all_commit : Context_default_dispatcher
    {
        #region Estabilish value

        static CommandComposite composite;

        #endregion

        Establish establish = () =>
                                  {
                                      var message = Pleasure.Mock<CommandBase>();
                                      var message2 = Pleasure.Mock<CommandBase>();

                                      composite = new CommandComposite();
                                      composite.Quote(message.Object, new MessageExecuteSetting { Commit = true });
                                      composite.Quote(message2.Object, new MessageExecuteSetting { Commit = true });
                                  };

        Because of = () => dispatcher.Push(composite);

        It should_be_commit = () => unitOfWork.Verify(r => r.Commit(), Times.Exactly(2));

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose(), Times.Once());

        It should_be_committed = () => unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<IDbConnection>()));
    }
}