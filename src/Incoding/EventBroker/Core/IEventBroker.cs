#region copyright

// @incoding 2011
#endregion

namespace Incoding.EventBroker
{
    public interface IEventBroker
    {
        void Publish<TEvent>(TEvent @event) where TEvent : class, IEvent;
    }
}