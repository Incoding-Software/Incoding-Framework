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
    public class When_default_dispatcher_push_composite_with_setting_exception : Context_default_dispatcher
    {
        #region Establish value

        static CommandComposite composite;

        static Mock<ISpy> onSpy;

        static Exception exception;

        #endregion

        Establish establish = () =>
                                  {
                                      var message = Pleasure.Mock<CommandBase>(mock => mock.Setup(r => r.Execute()).Throws(new IncFakeException()));

                                      onSpy = Pleasure.Spy();

                                      composite = new CommandComposite();
                                      composite.Quote(message.Object)
                                               .OnBefore(r => onSpy.Object.Is(r, "onBefore"))
                                               .OnError((r, ex) => onSpy.Object.Is(r, ex, "onError"))
                                               .OnComplete(r => onSpy.Object.Is(r, "onComplete"));
                                  };

        Because of = () => { exception = Catch.Exception(() => dispatcher.Push(composite)); };

        It should_be_throw = () => exception.ShouldBeOfType<IncFakeException>();

        It should_be_on_before = () => onSpy.Verify(r => r.Is(Pleasure.MockIt.IsAny<CommandBase>(), "onBefore"), Times.Once());

        It should_be_on_error = () => onSpy.Verify(r => r.Is(Pleasure.MockIt.IsAny<CommandBase>(), Pleasure.MockIt.IsAny<IncFakeException>(), "onError"), Times.Once());

        It should_be_on_complete = () => onSpy.Verify(r => r.Is(Pleasure.MockIt.IsAny<CommandBase>(), "onComplete"), Times.Once());
    }
}