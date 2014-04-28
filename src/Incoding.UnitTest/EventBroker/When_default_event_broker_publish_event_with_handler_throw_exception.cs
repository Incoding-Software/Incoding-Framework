namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Block.IoC;
    using Incoding.EventBroker;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
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

        #region Establish value

        static Exception exception;

        static Mock<ISpy> eventHandlerSpy;

        static DefaultEventBroker eventBroker;

        #endregion

        Establish establish = () =>
                                  {
                                      eventBroker = new DefaultEventBroker();
                                      eventHandlerSpy = Pleasure.Spy(mock => mock.Setup(r => r.Is(Pleasure.MockIt.IsAny<object[]>())).Throws<IncFakeException>());

                                      var fakeEventSubscriberWithException = new FakeEventSubscriberWithException(eventHandlerSpy.Object);
                                      var mockIoCProvider = Pleasure.Mock<IIoCProvider>(mock => mock.Setup(r => r.GetAll<object>(typeof(IEventSubscriber<>).MakeGenericType(new[] { typeof(FakeEventWithException) }))).Returns(new List<object> { fakeEventSubscriberWithException }));
                                      IoCFactory.Instance.Initialize(init => init.WithProvider(mockIoCProvider.Object));
                                  };

        Because of = () => { exception = Catch.Exception(() => eventBroker.Publish(new FakeEventWithException())); };

        It should_be_catch_exception = () => exception.ShouldNotBeNull();

        It should_be_disposable = () => eventHandlerSpy.Verify(r => r.Is(Pleasure.MockIt.Is<object[]>(o => o[0].ShouldEqual("Dispose"))));
    }
}