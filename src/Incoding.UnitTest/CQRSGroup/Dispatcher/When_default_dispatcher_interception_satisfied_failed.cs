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
    public class When_default_dispatcher_interception_satisfied_failed : Context_default_dispatcher
    {
        #region Establish value

        static CommandWithThrow message;

        #endregion

        private static Mock<IMessageInterception> interception;

        private static Exception exception;

        Establish establish = () =>
                              {
                                  message = Pleasure.Generator.Invent<CommandWithThrow>();
                                  interception = Pleasure.MockStrict<IMessageInterception>(mock =>
                                                                                           {
                                                                                               var context = new MessageInterceptionContext()
                                                                                                             {
                                                                                                                     Dispatcher = dispatcher,
                                                                                                                     Message = message
                                                                                                             };
                                                                                               mock.Setup(r => r.IsSatisfied(Pleasure.MockIt.IsStrong(context))).Returns(true);
                                                                                               mock.Setup(r => r.OnBefore(Pleasure.MockIt.IsStrong(context)));
                                                                                               mock.Setup(r => r.OnError(Pleasure.MockIt.IsStrong(context), Pleasure.MockIt.IsAny<ArgumentException>()));
                                                                                               mock.Setup(r => r.OnComplete(Pleasure.MockIt.IsStrong(context)));
                                                                                           });
                                  DefaultDispatcher.SetInterception(interception.Object);
                              };

        Because of = () => { exception = Catch.Exception(() => dispatcher.Push(message)); };

        It should_be_ex = () => exception.ShouldNotBeNull();

        It should_be_interception = () => interception.VerifyAll();
    }
}