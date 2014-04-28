namespace Incoding.EventBroker
{
    #region << Using >>

    using System;

    #endregion

    public interface IEventSubscriber<in TEvent> : IDisposable where TEvent : IEvent
    {
        void Subscribe(TEvent @event);
    }
}