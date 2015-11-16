namespace Incoding.EventBroker
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Reflection;
    using Incoding.Block.IoC;
    using Incoding.Extensions;

    #endregion

    public class DefaultEventBroker : IEventBroker
    {
        #region IEventBroker Members

        public void Publish<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            if (!HasSubscriber(@event))
                return;

            var eventType = @event.GetType();

            var allSubscriberTypes = IoCFactory.Instance.ResolveAll<object>(typeof(IEventSubscriber<>).MakeGenericType(new[] { eventType }));

            foreach (var currentHandler in allSubscriberTypes)
            {
                var handleMethod = currentHandler
                        .GetType()
                        .GetMethods()
                        .First(r => r.Name.Equals("Subscribe") && r.GetParameters().Any(parameterInfo => parameterInfo.ParameterType == eventType));

                var eventHandler = currentHandler;

                Action handleEvent = () =>
                                     {
                                         try
                                         {
                                             handleMethod.Invoke(eventHandler, new object[] { @event });
                                         }
                                         catch (TargetInvocationException invocationException)
                                         {
                                             var actualException = invocationException.InnerException;
                                             throw actualException;
                                         }
                                         finally
                                         {
                                             ((IDisposable)eventHandler).Dispose();
                                         }
                                     };

                if (handleMethod.HasAttribute<HandlerAsyncAttribute>())
                    handleEvent.BeginInvoke(null, handleEvent);
                else
                    handleEvent.Invoke();
            }
        }

        #endregion

        bool HasSubscriber<TEvent>(TEvent onEvent) where TEvent : IEvent
        {
            var subscriberType = typeof(IEventSubscriber<>).MakeGenericType(new[] { typeof(TEvent) });
            return IoCFactory.Instance
                             .ResolveAll<object>(subscriberType)
                             .Any();
        }
    }
}