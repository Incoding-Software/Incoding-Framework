namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Data;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_with_throw_exception_and_re_throw_in_event
    {
        #region Fake classes

        [Serializable]
        class MyException : Exception { }

        class DispatcherSubscriber : IEventSubscriber<OnAfterErrorExecuteEvent>
        {
            #region IEventSubscriber<OnAfterErrorExecuteEvent> Members

            public void Subscribe(OnAfterErrorExecuteEvent @event)
            {
                throw @event.Exception;
            }

            #endregion

            #region Disposable

            public void Dispose() { }

            #endregion
        }

        #endregion

        #region Establish value

        static Mock<CommandBase> message;

        static Exception exception;

        static DefaultDispatcher dispatcher;

        #endregion

        Establish establish = () =>
                                  {
                                      IoCFactory.Instance.Initialize(init => init.WithProvider(Pleasure.MockAsObject<IIoCProvider>(mock =>
                                                                                                                                       {
                                                                                                                                           var unitOfWorkFactory = Pleasure.Mock<IUnitOfWorkFactory>(unitOfWorkFactoryMock =>
                                                                                                                                                                                                         {
                                                                                                                                                                                                             var unitOfWork = new Mock<IUnitOfWork>();
                                                                                                                                                                                                             unitOfWorkFactoryMock
                                                                                                                                                                                                                     .Setup(r => r.Create(Pleasure.MockIt.IsAny<IsolationLevel>(), Pleasure.MockIt.IsNull<string>()))
                                                                                                                                                                                                                     .Returns(unitOfWork.Object);
                                                                                                                                                                                                         });

                                                                                                                                           mock.Setup(r => r.TryGet<IUnitOfWorkFactory>()).Returns(unitOfWorkFactory.Object);
                                                                                                                                           mock.Setup(r => r.Get<object>(typeof(DispatcherSubscriber))).Returns(new DispatcherSubscriber());
                                                                                                                                           mock.Setup(r => r.TryGet<IEventBroker>()).Returns(new DefaultEventBroker());
                                                                                                                                           mock.Setup(r => r.GetAll<object>(typeof(IEventSubscriber<>).MakeGenericType(new[] { typeof(OnAfterErrorExecuteEvent) }))).Returns(new List<object> { new DispatcherSubscriber() });
                                                                                                                                       })));

                                      message = Pleasure.Mock<CommandBase>(mock => mock.Setup(r => r.Execute()).Throws<MyException>());

                                      dispatcher = new DefaultDispatcher();
                                  };

        Because of = () => { exception = Catch.Exception(() => dispatcher.Push(message.Object)); };

        It should_be_re_throw = () => exception.ShouldBeOfType<MyException>();
    }
}