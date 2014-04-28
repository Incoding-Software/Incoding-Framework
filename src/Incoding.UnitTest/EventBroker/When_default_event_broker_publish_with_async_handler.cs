namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Block.IoC;
    using Incoding.EventBroker;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DefaultEventBroker))]
    public class When_default_event_broker_publish_with_async_handler
    {
        #region Fake classes

        class FakeEventAsync : IEvent { }

        class AsyncEventSubscriber : IEventSubscriber<FakeEventAsync>
        {
            #region Static Fields

            [ThreadStatic]
            public static bool IsOnlyForThreadEvent;

            public static bool ForAll;

            #endregion

            #region IEventSubscriber<FakeEventAsync> Members

            [HandlerAsync]
            public void Subscribe(FakeEventAsync @event)
            {
                IsOnlyForThreadEvent = true;
                ForAll = true;
            }

            #endregion

            #region Disposable

            public void Dispose() { }

            #endregion
        }

        #endregion

        #region Establish value

        static DefaultEventBroker eventBroker;

        #endregion

        Establish establish = () =>
                                  {
                                      eventBroker = new DefaultEventBroker();

                                      var mockIoCProvider = Pleasure.MockAsObject<IIoCProvider>(mock => mock
                                                                                                                .Setup(r => r.GetAll<object>(typeof(IEventSubscriber<>).MakeGenericType(new[] { typeof(FakeEventAsync) })))
                                                                                                                .Returns(new List<object> { new AsyncEventSubscriber() }));

                                      IoCFactory.Instance.Initialize(init => init.WithProvider(mockIoCProvider));
                                  };

        Because of = () =>
                         {
                             eventBroker.Publish(new FakeEventAsync());
                             Pleasure.Sleep50Milliseconds();
                         };

        It should_be_for_all = () => AsyncEventSubscriber.ForAll.ShouldBeTrue();

        It should_be_handle_in_different_thread = () => AsyncEventSubscriber.IsOnlyForThreadEvent.ShouldBeFalse();
    }
}