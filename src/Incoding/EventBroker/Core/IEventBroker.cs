#region copyright

// @incoding 2011
#endregion

namespace Incoding.EventBroker
{
    using Incoding.CQRS;

    public interface IEventBroker
    {
        #region Methods

        void Publish<TEvent>(TEvent @event) where TEvent : class, IEvent;

        #endregion

        bool HasSubscriber<TEvent>(TEvent onAfterErrorExecuteEvent) where TEvent : IEvent;

    }
}