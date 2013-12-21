namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.EventBroker;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultEventBroker))]
    public class When_default_event_broker_with_aggregate_subscriber
    {
        #region Fake classes

        interface IAggregateEvent : IEvent { }

        class AggregateEvent : IAggregateEvent { }

        class AggregateEvent2 : IAggregateEvent { }

        class AggregateEvent3 : IAggregateEvent { }

        class AggregateSubscriber : IEventSubscriber<IEvent>
        {
            #region Fields

            readonly ISpy spy;

            #endregion

            #region Constructors

            public AggregateSubscriber(ISpy spy)
            {
                this.spy = spy;
            }

            #endregion

            #region IEventSubscriber<IEvent> Members

            public void Subscribe(IEvent @event)
            {
                this.spy.Is(@event);
            }

            #endregion

            #region Disposable

            public void Dispose() { }

            #endregion
        }

        #endregion

        #region Estabilish value

        static DefaultEventBroker eventBroker;

        static Mock<ISpy> spy;

        #endregion

        Establish establish = () =>
                                  {
                                      spy = Pleasure.Spy();

                                      eventBroker = new DefaultEventBroker()
                                              .WithSharedSubscriber(() => new AggregateSubscriber(spy.Object));
                                  };

        Because of = () =>
                         {
                             eventBroker.Publish(new AggregateEvent());
                             eventBroker.Publish(new AggregateEvent2());
                             eventBroker.Publish(new AggregateEvent3());
                         };

        It should_be_subscribe_for_each = () => spy.Verify(r => r.Is(Pleasure.MockIt.IsAny<IEvent>()), Times.Exactly(3));
    }
}