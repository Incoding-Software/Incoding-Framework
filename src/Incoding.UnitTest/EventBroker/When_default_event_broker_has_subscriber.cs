namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.EventBroker;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DefaultEventBroker))]
    public class When_default_event_broker_has_subscriber
    {
        #region Fake classes

        class FakeEvent : IEvent { }

        class FakeEventWithoutSubscriber : IEvent { }

        #endregion

        #region Estabilish value

        static DefaultEventBroker eventBroker;

        #endregion

        Establish establish = () =>
                                  {
                                      eventBroker = new DefaultEventBroker();

                                      IoCFactory.Instance.Stub(mock =>
                                                                   {
                                                                       mock.Setup(r => r.GetAll<object>(typeof(IEventSubscriber<FakeEvent>))).Returns(new object[] { 1, 2 });
                                                                       mock.Setup(r => r.GetAll<object>(typeof(IEventSubscriber<FakeEventWithoutSubscriber>))).Returns(new object[] { });
                                                                   });
                                  };

        It should_be_has_subscribers = () => eventBroker.HasSubscriber(new FakeEvent()).ShouldBeTrue();

        It should_not_be_has_subscribers = () => eventBroker.HasSubscriber(new FakeEventWithoutSubscriber()).ShouldBeFalse();
    }
}