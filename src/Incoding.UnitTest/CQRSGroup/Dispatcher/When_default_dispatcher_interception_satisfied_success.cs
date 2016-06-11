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
    public class When_default_dispatcher_interception_satisfied_success : Context_default_dispatcher
    {
        #region Establish value

        static CommandWithoutRepository message;

        #endregion

        private static Mock<IMessageInterception> interception;

        Establish establish = () =>
                              {
                                  message = Pleasure.Generator.Invent<CommandWithoutRepository>();
                                  interception = Pleasure.MockStrict<IMessageInterception>(mock =>
                                                                                           {
                                                                                               var context = new MessageInterceptionContext()
                                                                                                             {
                                                                                                                     Dispatcher = dispatcher,
                                                                                                                     Message = message
                                                                                                             };
                                                                                               mock.Setup(r => r.IsSatisfied(Pleasure.MockIt.IsStrong(context))).Returns(true);
                                                                                               mock.Setup(r => r.OnBefore(Pleasure.MockIt.IsStrong(context)));
                                                                                               mock.Setup(r => r.OnSuccess(Pleasure.MockIt.IsStrong(context)));
                                                                                               mock.Setup(r => r.OnComplete(Pleasure.MockIt.IsStrong(context)));
                                                                                           });
                                  DefaultDispatcher.SetInterception(interception.Object);
                              };

        Because of = () => dispatcher.Push(message);

        It should_be_interception = () => interception.VerifyAll();
    }
}