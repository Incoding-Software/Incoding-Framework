namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Block.IoC;
    using Incoding.EventBroker;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(DefaultEventBroker))]
    public class When_default_event_broker_publish_with_async_handler_and_wait
    {
        #region Fake classes

        class FakeEventAsyncSleep : IEvent { }

        class SleepAsyncEventSubscriber : IEventSubscriber<FakeEventAsyncSleep>
        {
            #region Static Fields

            public static bool SetAfterThreadSleep;

            #endregion

            #region IEventSubscriber<FakeEventAsyncSleep> Members

            [HandlerAsyncWait]
            public void Subscribe(FakeEventAsyncSleep @event)
            {
                Pleasure.SleepMilliseconds(200);
                SetAfterThreadSleep = true;
            }

            #endregion

            #region Disposable

            public void Dispose() { }

            #endregion
        }

        #endregion

        #region Estabilish value

        static DefaultEventBroker eventBroker;

        #endregion

        Establish establish = () =>
                                  {
                                      eventBroker = new DefaultEventBroker();
                                      var mockIoCProvider = Pleasure.MockAsObject<IIoCProvider>(mock => mock
                                                                                                                .Setup(r => r.GetAll<object>(typeof(IEventSubscriber<>).MakeGenericType(new[] { typeof(FakeEventAsyncSleep) })))
                                                                                                                .Returns(new List<object> { new SleepAsyncEventSubscriber() }));

                                      IoCFactory.Instance.Initialize(init => init.WithProvider(mockIoCProvider));
                                  };

        Because of = () => eventBroker.Publish(new FakeEventAsyncSleep());

        It should_be_wait = () => SleepAsyncEventSubscriber.SetAfterThreadSleep.ShouldBeTrue();
    }
}