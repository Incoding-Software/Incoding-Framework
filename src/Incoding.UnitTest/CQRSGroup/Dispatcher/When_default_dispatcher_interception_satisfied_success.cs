namespace Incoding.UnitTest
{
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_interception_satisfied_success : Context_default_dispatcher
    {
        #region Establish value

        static CommandWithoutRepository message;

        #endregion

        private static Mock<DefaultDispatcher.IMessageInterception> interception;

        Establish establish = () =>
                              {
                                  message = Pleasure.Generator.Invent<CommandWithoutRepository>();
                                  interception = Pleasure.MockStrict<DefaultDispatcher.IMessageInterception>(mock =>
                                                                                                             {
                                                                                                                 mock.Setup(r => r.IsSatisfied(Pleasure.MockIt.IsStrong(message))).Returns(true);
                                                                                                                 mock.Setup(r => r.OnBefore(Pleasure.MockIt.IsStrong(message)));
                                                                                                                 mock.Setup(r => r.OnSuccess(Pleasure.MockIt.IsStrong(message)));
                                                                                                                 mock.Setup(r => r.OnComplete(Pleasure.MockIt.IsStrong(message)));
                                                                                                             });
                                  dispatcher.SetInterception(interception.Object);
                              };

        Because of = () => dispatcher.Push(message);

        It should_be_interception = () => interception.VerifyAll();
    }
}