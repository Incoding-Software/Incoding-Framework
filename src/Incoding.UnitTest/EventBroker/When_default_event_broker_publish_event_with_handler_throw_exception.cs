namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Block.ExceptionHandling;
    using Incoding.Block.IoC;
    using Incoding.EventBroker;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultEventBroker))]
    public class When_default_event_broker_publish_event_with_handler_throw_exception
    {
        #region Fake classes

        class FakeEventSubscriberWithException : IEventSubscriber<FakeEventWithException>
        {
            #region Fields

            readonly ISpy spy;

            #endregion

            #region Constructors

            public FakeEventSubscriberWithException(ISpy spy)
            {
                this.spy = spy;
            }

            #endregion

            #region IEventSubscriber<FakeEventWithException> Members

            public void Subscribe(FakeEventWithException @event)
            {
                this.spy.Is("Handle", @event);
            }

            #endregion

            #region Disposable

            public void Dispose()
            {
                this.spy.Is("Dispose");
            }

            #endregion
        }

        class FakeEventWithException : IEvent { }

        #endregion

        #region Estabilish value

        static Exception exception;

        static Mock<ISpy> actionPolicySpy;

        static Mock<ISpy> eventHandlerSpy;

        static DefaultEventBroker eventBroker;

        #endregion

        Establish establish = () =>
                                  {
                                      actionPolicySpy = Pleasure.Spy();
                                      eventBroker = new DefaultEventBroker()
                                              .WithActionPolicy(ActionPolicy.Catch(r => actionPolicySpy.Object.Is(r)));
                                      eventHandlerSpy = Pleasure.Spy(mock => mock.Setup(r => r.Is(Pleasure.MockIt.IsAny<object[]>())).Throws<IncFakeException>());

                                      var fakeEventSubscriberWithException = new FakeEventSubscriberWithException(eventHandlerSpy.Object);
                                      var mockIoCProvider = Pleasure.Mock<IIoCProvider>(mock => mock.Setup(r => r.GetAll<object>(typeof(IEventSubscriber<>).MakeGenericType(new[] { typeof(FakeEventWithException) }))).Returns(new List<object> { fakeEventSubscriberWithException }));
                                      IoCFactory.Instance.Initialize(init => init.WithProvider(mockIoCProvider.Object));
                                  };

        Because of = () => { exception = Catch.Exception(() => eventBroker.Publish(new FakeEventWithException())); };

        It should_be_catch_exception = () => exception.ShouldBeNull();

        It should_be_disposable = () => eventHandlerSpy.Verify(r => r.Is(Pleasure.MockIt.Is<object[]>(o => o[0].ShouldEqual("Dispose"))));

        It should_be_execute_action_policy = () => actionPolicySpy.Verify(r => r.Is(Pleasure.MockIt.Is<object[]>(objects => objects[0].ShouldBeOfType<IncFakeException>())));
    }
}