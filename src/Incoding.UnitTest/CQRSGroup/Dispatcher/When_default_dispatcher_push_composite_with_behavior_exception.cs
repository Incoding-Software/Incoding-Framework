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
    public class When_default_dispatcher_push_composite_with_behavior_exception : Context_default_dispatcher
    {
        #region Estabilish value

        static CommandComposite composite;

        static Mock<ISpy> onSpy;

        #endregion

        Establish establish = () =>
                                  {
                                      var message = Pleasure.Mock<CommandBase>(mock => mock.Setup(r => r.Execute()).Throws(new IncFakeException()));

                                      onSpy = Pleasure.Spy();

                                      composite = new CommandComposite();
                                      composite.Quote(message.Object, new MessageExecuteSetting
                                                                          {
                                                                                  OnComplete = () => onSpy.Object.Is("onComplete"), 
                                                                                  OnBefore = () => onSpy.Object.Is("onBefore"), 
                                                                                  OnError = (exception) => onSpy.Object.Is("onError", exception)
                                                                          });
                                      eventBroker.Setup(r => r.HasSubscriber(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>())).Returns(true);
                                  };

        Because of = () => dispatcher.Push(composite);

        It should_be_on_before = () => onSpy.Verify(r => r.Is("onBefore"), Times.Once());

        It should_be_on_error = () => onSpy.Verify(r => r.Is("onError", Pleasure.MockIt.IsAny<IncFakeException>()), Times.Once());

        It should_be_on_complete = () => onSpy.Verify(r => r.Is("onComplete"), Times.Once());
    }
}