namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_with_throw_exception_and_re_throw_in_event
    {
        Establish establish = () =>
                              {
                                  message = Pleasure.Mock<CommandBase>(mock =>
                                                                       {
                                                                           mock.Setup(r => r.OnExecute(Pleasure.MockIt.IsAny<IDispatcher>(), Pleasure.MockIt.IsAny<Lazy<IUnitOfWork>>())).Throws<MyException>();
                                                                           mock.Setup(r => r.Setting).ReturnsInvent();
                                                                       });

                                  dispatcher = new DefaultDispatcher();
                              };

        Because of = () => { exception = Catch.Exception(() => dispatcher.Push(message.Object)); };

        It should_be_re_throw = () => exception.ShouldBeAssignableTo<MyException>();

        #region Fake classes

        [Serializable]
        class MyException : Exception { }

        #endregion

        #region Establish value

        static Mock<CommandBase> message;

        static Exception exception;

        static DefaultDispatcher dispatcher;

        #endregion
    }
}