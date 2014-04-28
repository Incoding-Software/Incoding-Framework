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
    public class When_default_dispatcher_push_composite : Context_default_dispatcher
    {
        #region Establish value

        static CommandComposite composite;

        static Mock<ISpy> onSpy;

        #endregion

        Establish establish = () =>
                                  {
                                      var message = Pleasure.Mock<CommandBase>();

                                      onSpy = Pleasure.Spy();

                                      composite = new CommandComposite();
                                      composite.Quote(message.Object)
                                               .OnBefore(r => onSpy.Object.Is(r, "onBefore"))
                                               .OnComplete(r => onSpy.Object.Is(r, "onComplete"))
                                               .OnAfter(r => onSpy.Object.Is(r, "onAfter"));
                                  };

        Because of = () => dispatcher.Push(composite);

        It should_be_on_before = () => onSpy.Verify(r => r.Is(Pleasure.MockIt.IsAny<CommandBase>(), "onBefore"), Times.Once());

        It should_be_on_after = () => onSpy.Verify(r => r.Is(Pleasure.MockIt.IsAny<CommandBase>(), "onAfter"), Times.Once());

        It should_be_on_complete = () => onSpy.Verify(r => r.Is(Pleasure.MockIt.IsAny<CommandBase>(), "onComplete"), Times.Once());
    }
}