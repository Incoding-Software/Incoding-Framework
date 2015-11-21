namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.EventBroker;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Behaviors]
    public class Behaviors_default_event_broker_publish_event
    {
        #region Establish value

        protected static DefaultEventBroker eventBroker;

        protected static void Establish(IIoCProvider ioCProvider)
        {
            FakeEventSubscriber.Spy = Pleasure.Spy();
            eventBroker = new DefaultEventBroker();
            IoCFactory.Instance.Initialize(init => init.WithProvider(ioCProvider));
        }

        #endregion

        #region Nested classes

        public class FakeEvent : IEvent { }

        public class FakeEvent2 : IEvent { }

        public class FakeEventSubscriber : IEventSubscriber<FakeEvent2>, IEventSubscriber<FakeEvent>
        {
            #region Static Fields

            public static Mock<ISpy> Spy = Pleasure.Spy();

            #endregion

            #region IEventSubscriber<FakeEvent> Members

            public void Subscribe(FakeEvent @event)
            {
                Spy.Object.Is("Handle", @event);
            }

            #endregion

            #region IEventSubscriber<FakeEvent2> Members

            public void Subscribe(FakeEvent2 @event)
            {
                Spy.Object.Is("Handle", @event);
            }

            #endregion

            #region Disposable

            public void Dispose()
            {
                Spy.Object.Is("Dispose");
            }

            #endregion
        }

        #endregion

        It should_be_execute_fake_1 = () => FakeEventSubscriber.Spy.Verify(r => r.Is(Pleasure.MockIt.IsStrong(new object[] { "Handle", new FakeEvent() })), Times.Once());

        It should_be_execute_fake_2 = () => FakeEventSubscriber.Spy.Verify(r => r.Is(Pleasure.MockIt.IsStrong(new object[] { "Handle", new FakeEvent2() })), Times.Once());

        It should_be_disposable = () => FakeEventSubscriber.Spy.Verify(r => r.Is(Pleasure.MockIt.IsStrong(new object[] { "Dispose" })), Times.Exactly(2));
    }
}