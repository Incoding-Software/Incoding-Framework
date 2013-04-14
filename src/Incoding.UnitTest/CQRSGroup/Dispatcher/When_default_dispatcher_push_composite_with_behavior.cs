namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.CQRS;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_composite_with_behavior : Context_default_dispatcher
    {
        #region Estabilish value

        static CommandComposite composite;

        static Mock<ISpy> onSpy;

        #endregion

        Establish establish = () =>
                                  {
                                      var message = Pleasure.Mock<CommandBase>();

                                      onSpy = Pleasure.Spy();

                                      composite = new CommandComposite();
                                      composite.Quote(message.Object, new MessageExecuteSetting
                                                                          {
                                                                                  OnComplete = () => onSpy.Object.Is("onComplete"), 
                                                                                  OnBefore = () => onSpy.Object.Is("onBefore"), 
                                                                                  OnAfter = () => onSpy.Object.Is("onAfter"), 
                                                                          });
                                  };

        Because of = () => dispatcher.Push(composite);

        It should_be_on_before = () => onSpy.Verify(r => r.Is("onBefore"), Times.Once());

        It should_be_on_after = () => onSpy.Verify(r => r.Is("onAfter"), Times.Once());

        It should_be_on_complete = () => onSpy.Verify(r => r.Is("onComplete"), Times.Once());
    }
}