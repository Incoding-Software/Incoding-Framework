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
    public class When_default_dispatcher_interception_not_satisfied : Context_default_dispatcher
    {
        #region Establish value

        static CommandWithRepository message;

        #endregion

        private static Mock<IMessageInterception> interception;

        Establish establish = () =>
                              {
                                  message = Pleasure.Generator.Invent<CommandWithRepository>();
                                  interception = Pleasure.MockStrict<IMessageInterception>(mock => { mock.Setup(r => r.IsSatisfied(Pleasure.MockIt.IsStrong(new MessageInterceptionContext()
                                                                                                                                                            {
                                                                                                                                                                    Dispatcher = dispatcher,
                                                                                                                                                                    Message = message
                                                                                                                                                            }))).Returns(false); });
                                  DefaultDispatcher.SetInterception(interception.Object);
                              };

        Because of = () => dispatcher.Push(message);

        It should_be_interception = () => interception.VerifyAll();
    }
}