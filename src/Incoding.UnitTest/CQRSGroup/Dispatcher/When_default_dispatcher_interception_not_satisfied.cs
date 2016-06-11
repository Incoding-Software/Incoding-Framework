namespace Incoding.UnitTest
{
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_interception_not_satisfied : Context_default_dispatcher
    {
        #region Establish value

        static CommandWithRepository message;

        #endregion

        private static Mock<DefaultDispatcher.IMessageInterception> interception;

        Establish establish = () =>
                              {
                                  message = Pleasure.Generator.Invent<CommandWithRepository>();
                                  interception = Pleasure.MockStrict<DefaultDispatcher.IMessageInterception>(mock =>
                                                                                                             {
                                                                                                                 mock.Setup(r => r.IsSatisfied(Pleasure.MockIt.IsStrong(message))).Returns(false);                                                                                                                 
                                                                                                             });
                                  dispatcher.SetInterception(interception.Object);
                              };

        Because of = () => dispatcher.Push(message);

        It should_be_interception = () => interception.VerifyAll();
    }
}