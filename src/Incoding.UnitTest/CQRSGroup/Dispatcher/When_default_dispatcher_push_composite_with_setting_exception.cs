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
        Establish establish = () =>
                              {
                                  var message = Pleasure.Mock<CommandBase>(mock => mock.Setup(r => r.OnExecute(dispatcher, unitOfWork.Object)).Throws(new IncFakeException()));

                                  onSpy = Pleasure.Spy();

                                  composite = new CommandComposite();
                                  composite.Quote(message.Object);
                              };

        Because of = () => { exception = Catch.Exception(() => dispatcher.Push(composite)); };

        It should_be_throw = () => exception.ShouldBeOfType<IncFakeException>();

        #region Establish value

        static CommandComposite composite;

        static Mock<ISpy> onSpy;

        static Exception exception;

        #endregion
    }
}