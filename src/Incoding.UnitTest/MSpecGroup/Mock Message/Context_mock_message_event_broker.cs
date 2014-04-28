namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.EventBroker;
    using Incoding.MSpecContrib;

    #endregion

    public class Context_mock_message_event_broker
    {
        #region Fake classes

        protected class FakeEvent : IEvent
        {
            #region Properties

            public string Value { get; set; }

            #endregion
        }

        protected class FakeCommand : CommandBase
        {
            public override void Execute()
            {
                EventBroker.Publish(new FakeEvent { Value = Pleasure.Generator.TheSameString() });
            }
        }

        #endregion

        #region Establish value

        protected static MockMessage<FakeCommand, object> mockMessage;

        protected static Exception exception;

        #endregion
    }
}