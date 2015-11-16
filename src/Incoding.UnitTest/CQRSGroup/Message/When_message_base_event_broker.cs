namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(MessageBase))]
    public class When_message_base_event_broker
    {
        #region Fake classes

        class FakeEvent : IEvent { }

        class FakeMessage : MessageBase
        {
            protected override void Execute()
            {
                Pleasure.Do10(i => EventBroker.Publish(new FakeEvent()));
            }
        }

        #endregion

        #region Establish value

        static FakeMessage message;

        static Mock<IIoCProvider> provider;

        #endregion

        Establish establish = () =>
                              {
                                  provider = Pleasure.MockStrict<IIoCProvider>(mock => mock.Setup(r => r.TryGet<IEventBroker>()).Returns(Pleasure.MockAsObject<IEventBroker>()));
                                  IoCFactory.Instance.Initialize(init => init.WithProvider(provider.Object));
                                  message = new FakeMessage();
                              };

        Because of = () => message.OnExecute(Pleasure.MockStrictAsObject<IDispatcher>(), new Lazy<IUnitOfWork>(() => Pleasure.MockStrictAsObject<IUnitOfWork>()));

        It should_be_resolve_once = () => provider.Verify(r => r.TryGet<IEventBroker>(), Times.Once());
    }
}